using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

class Node
{	
	public Node next = null;//our neighbor, used for iterating
	public NodeLink links = null;//the links that connect us to other nodes
	//if links is null, we assume we are the final node
	
	public double value = 0;//stores our sum
	//used for the frontmost nodes to get game data
	//used at the end as the calculated values

	public Node()
	{
		
	}
	
	public void calculate()//calculate until we are that very end with no nodes
	{
		//calculate our data
		//run through all links
		NodeLink linkIterator = links;
		
		//add the dot product for each one
		while(linkIterator!=null)
		{
			linkIterator.calculate();
			
			linkIterator = linkIterator.next;
		}
	}
}

class NodeLink
{
	double weight = 1.0;
	Node destination = null;
	Node origin = null;
	public NodeLink next = null;//used for iterating across links
	
	public NodeLink( Node orig, Node dest, double nweight)
	{
		destination = dest;
		origin = orig;
		weight = nweight;
	}
	
	public void calculate()//calculate until we are that very end with no nodes
	{
		//multiply output on origin by the weight and add to value at destination
		destination.value += weight * origin.value;
	}
	
	public void clear()
	{
		destination = null;
		origin = null;
	}
}

public class AiBase : MonoBehaviour
{
    public PlayerController ourPlayer;
	
		//Our neural network stuff is held here
	Node nodeLayerInput = null;
		//We have our position on the map
		//Our character
		//Our active items
		//Our passives
		//Our cooldowns
		//Our health, max health
		//Our mana, max mana
		//Time since last jump (jump cooldown)
		//Other player data
			//Position (x, y)
			//Distance from us (dx, dy)
			//health, max health
			//mana, max mana
			//Character (int for character id with gaps of 1000, no player is -1000)
			//Attack/item cooldowns
			//Active items (int for each item id * 100)
			//Passive items (int for each item id * 100, -1 for no passive here)
			//Time since last jump (jump cooldown)
	int inputLayerCount = 0;
	Node nodeLayer1 = null;//20 of these
	int layer1Count = 0;
	Node nodeLayer2 = null;//20 of these
	int layer2Count = 0;
	Node nodeLayerOutput = null;
	int outputLayerCount = 0;
	
	int ticksToDestination = 0;
	
	int directionCount = 0;
	
	public AiTrainerManager aiBroodmother = null;
		
	public double score = 0;//this is our performance with this genetic layout, higher score = better
		//modified during gameplay at real-time
	
	//scoring data
		//use this data for calculating the score, such as trickshots
		//may be useful for stats screen?
	
	protected double aiLastDirection = 0;
	
	double desiredX = 0;
	double desiredY = 0;

    void Start ()
    {
    }
	
    void Update ()
    {
		if(ourPlayer!=null)
		{
			//get game status info
			pollInputs();
			
			calculateOutputs();//run data through the machine
			
			//use the outputs of the network
			readOutputs();
			
			makeDecisions();
			
			//check if the ai is going the same direction as before
			if(aiLastDirection == ourPlayer.aiDirection)
			{
				//if so, penalize it proportional to the time spent in that direction
				directionCount++;
				
				score -= directionCount / 100;
			}
			else
			{
				score+= 25;//small reward for mixing it out
				directionCount = 0;
			}
			
			//do not touch it if we aren't able to do anything
			//proc movement to the left just because
			if(ourPlayer.aiIdle)
				aiLastDirection = 0;
			else
				aiLastDirection = ourPlayer.aiDirection;
			
			//penalize straying from center of stage
			double distance = Math.Pow(ourPlayer.transform.position.x - 18,2);
			score -= distance/500;
		}
	}
	
	
	//this function will create our neural network, must pass in ALL weights needed here
	//all weights are from name->nextLayer
	public void createNetwork(double[] inputWeights, double[] layer1Weights, double[] layer2Weights)
	{
		//destroy our network before we can create a new network
		destroyNetwork();
		
		//start with creating nodes for each layer
		//input layer has 102 inputs between the four characters
		for(int i=0; i<inputLayerCount; i++)
		{
			//push them to the back for now
			var newNode = new Node();
			newNode.next = nodeLayerInput;
			nodeLayerInput = newNode;
		}
		//layer 1
		for(int i=0; i<layer1Count; i++)
		{
			//push them to the back for now
			var newNode = new Node();
			newNode.next = nodeLayer1;
			nodeLayer1 = newNode;
		}
		//layer 2
		for(int i=0; i<layer2Count; i++)
		{
			//push them to the back for now
			var newNode = new Node();
			newNode.next = nodeLayer2;
			nodeLayer2 = newNode;
		}
		//we have 9 outputs we need to worry about
		for(int i=0; i<outputLayerCount; i++)
		{
			//push them to the back for now
			var newNode = new Node();
			newNode.next = nodeLayerOutput;
			nodeLayerOutput = newNode;
		}
		
		//now create the links for it
		//four sets of weights
		//we will assume that we can continually keep pulling numbers without fear of going out of bounds until we have what we need
		//every node needs a link to every node in the next layer
		var iteratorLeft = nodeLayerInput;
		int index = 0;//this is our index in the weight array
		while(iteratorLeft!=null)
		{
			//iterate across all of the next layer
			var iteratorRight = nodeLayer1;
			while(iteratorRight != null)
			{
				//create link, push it onto the front
				var newLink = new NodeLink(iteratorLeft, iteratorRight, inputWeights[index]);
				newLink.next = iteratorLeft.links;
				iteratorLeft.links = newLink;
				
				iteratorRight = iteratorRight.next;
				index++;//up our index yos
			}
			
			iteratorLeft = iteratorLeft.next;
		}
		
		iteratorLeft = nodeLayer1;
		index = 0;//this is our index in the weight array
		while(iteratorLeft!=null)
		{
			//iterate across all of the next layer
			var iteratorRight = nodeLayer2;
			while(iteratorRight != null)
			{
				//create link, push it onto the front
				var newLink = new NodeLink(iteratorLeft, iteratorRight, layer1Weights[index]);
				newLink.next = iteratorLeft.links;
				iteratorLeft.links = newLink;
				
				iteratorRight = iteratorRight.next;
				index++;//up our index yos
			}
			
			iteratorLeft = iteratorLeft.next;
		}
		
		iteratorLeft = nodeLayer2;
		index = 0;//this is our index in the weight array
		while(iteratorLeft!=null)
		{
			//iterate across all of the next layer
			var iteratorRight = nodeLayerOutput;
			while(iteratorRight != null)
			{
				//create link, push it onto the front
				var newLink = new NodeLink(iteratorLeft, iteratorRight, layer2Weights[index]);
				newLink.next = iteratorLeft.links;
				iteratorLeft.links = newLink;
				
				iteratorRight = iteratorRight.next;
				index++;//up our index yos
			}
			
			iteratorLeft = iteratorLeft.next;
		}
	}
	
	//calculate what risk this target AI is
	double rateDanger(AiBase target)
	{
		if(target == null)
			return 0;//can't gauge a non-existent target
		
		double total = 300;//start it out here
		
		if(target.ourPlayer != null && ourPlayer != null)
		{
			
			//add potency if two items
			if(target.ourPlayer.active1 != null)
				total += 100;
			if(target.ourPlayer.active2 != null)
				total += 100;
			
			//add proximity value, small
			total += Math.Abs(36 - (target.ourPlayer.transform.position.x - ourPlayer.transform.position.x));
			total += Math.Abs(24 - (target.ourPlayer.transform.position.y - ourPlayer.transform.position.y))/2;
		
			total *= Math.Pow(10 * target.ourPlayer.currentHealth / target.ourPlayer.maxHealth,2);//multiply it by their current health, squared
			total *= 1 + (target.ourPlayer.currentMagic / target.ourPlayer.maxMagic);//factor in mana
			
			//check for invincibility
			if(target.ourPlayer.invincible)
				total = 999999;
			
			//check us for invincibility
			if(ourPlayer.invincible)
				total = 0;
		}
		
		return total;
	}
	
	void destroyNetwork()
	{
		if(nodeLayerInput == null || nodeLayer1 == null | nodeLayer2 == null | nodeLayerOutput == null)
		{
			return;//can't destroy what doesn't exist!
		}
		
		//reset our score
		score = 0;
		
		//delete our nodes
		while(nodeLayerInput != null)
		{
			//set each of our links to null
			while(nodeLayerInput.links != null)
			{
				nodeLayerInput.links.clear();
				
				nodeLayerInput.links = nodeLayerInput.links.next;
			}
			
			nodeLayerInput = nodeLayerInput.next;
		}
		
		while(nodeLayer1 != null)
		{
			//set each of our links to null
			while(nodeLayer1.links != null)
			{
				nodeLayer1.links.clear();
				
				nodeLayer1.links = nodeLayer1.links.next;
			}
			
			nodeLayer1 = nodeLayer1.next;
		}
		
		while(nodeLayer2 != null)
		{
			//set each of our links to null
			while(nodeLayer2.links != null)
			{
				nodeLayer2.links.clear();
				
				nodeLayer2.links = nodeLayer2.links.next;
			}
			
			nodeLayer2 = nodeLayer2.next;
		}
		
		while(nodeLayerOutput != null)
		{			
			nodeLayerOutput = nodeLayerOutput.next;
		}
	}
	
	void resetCalculations()
	{
		//reset calculations across all of the nodes
		var iterator = nodeLayerInput;
		
		while(iterator != null)
		{			
			iterator.value = 0;
	
			iterator = iterator.next;
		}
		
		iterator = nodeLayer1;
		while(iterator != null)
		{			
			iterator.value = 0;
	
			iterator = iterator.next;
		}
		
		iterator = nodeLayer2;
		while(iterator != null)
		{			
			iterator.value = 0;
	
			iterator = iterator.next;
		}
		
		iterator = nodeLayerOutput;
		while(iterator != null)
		{			
			iterator.value = 0;
	
			iterator = iterator.next;
		}
	}
	
	//this runs through our network to calculate individual outputs based on weights and the inputs
	void calculateOutputs()
	{
		resetCalculations();//need to reset all of the calculations before we make new ones
		
		
		//reset calculations across all of the nodes
		var iterator = nodeLayerInput;
		while(iterator != null)
		{			
			//calculate for each node
			iterator.calculate();
	
			iterator = iterator.next;
		}
		
		iterator = nodeLayer1;
		while(iterator != null)
		{			
			//make sure to squash our value
			iterator.value = squashFunction(iterator.value);
	
			//calculate for each node
			iterator.calculate();
	
			iterator = iterator.next;
		}
		
		iterator = nodeLayer2;
		while(iterator != null)
		{			
			//make sure to squash our value
			iterator.value = squashFunction(iterator.value);
	
			//calculate for each node
			iterator.calculate();
	
			iterator = iterator.next;
		}
		
		iterator = nodeLayerOutput;
		while(iterator != null)
		{			
			//make sure to squash our value
			iterator.value = squashFunction(iterator.value);
	
			iterator = iterator.next;
		}
	}
	
	void pollInputs()
	{
		if(ourPlayer == null)
			return;//don't run without a player
		
		Node currNode = null;
		//Time since last jump (jump cooldown)
		//Other player data
			//Distance from us (dx, dy)
			
		//later (low priority)
			//passive ids
		
		//grab other opponent data
		AiBase playerA = null;
		AiBase playerB = null;
		AiBase playerC = null;
		if(aiBroodmother.ai1 != this && aiBroodmother.ai1 != null)
		{
			if(playerA == null)
				playerA = aiBroodmother.ai1;
			else if(playerB == null)
				playerB = aiBroodmother.ai1;
			else
				playerC = aiBroodmother.ai1;
		}
		if(aiBroodmother.ai2 != this && aiBroodmother.ai2 != null)
		{
			if(playerA == null)
				playerA = aiBroodmother.ai2;
			else if(playerB == null)
				playerB = aiBroodmother.ai2;
			else
				playerC = aiBroodmother.ai2;
		}
		if(aiBroodmother.ai3 != this && aiBroodmother.ai3 != null)
		{
			if(playerA == null)
				playerA = aiBroodmother.ai3;
			else if(playerB == null)
				playerB = aiBroodmother.ai3;
			else
				playerC = aiBroodmother.ai3;
		}
		if(aiBroodmother.ai4 != this && aiBroodmother.ai4 != null)
		{
			if(playerA == null)
				playerA = aiBroodmother.ai4;
			else if(playerB == null)
				playerB = aiBroodmother.ai4;
			else
				playerC = aiBroodmother.ai4;
		}
		
		//get our x and y
		currNode = getNode(nodeLayerInput, 0);
		if(currNode != null)
			currNode.value = ourPlayer.transform.position.x;
		
		currNode = getNode(nodeLayerInput, 1);
		if(currNode != null)
			currNode.value = ourPlayer.transform.position.y;
		
		currNode = getNode(nodeLayerInput, 2);
		if(currNode != null)
			currNode.value = rateDanger(playerA);
		
		currNode = getNode(nodeLayerInput, 3);
		if(currNode != null)
			currNode.value = rateDanger(playerB);
		
		currNode = getNode(nodeLayerInput, 4);
		if(currNode != null)
			currNode.value = rateDanger(playerC);
		
		currNode = getNode(nodeLayerInput, 5);
		if(currNode != null)
			currNode.value = 1;//character ID, change later
		
		currNode = getNode(nodeLayerInput, 6);
		if(currNode != null)
			if(ourPlayer.active1 != null)
				currNode.value = ourPlayer.active1.id*10;//Item 1 ID
			else
				currNode.value = 0;
		
		currNode = getNode(nodeLayerInput, 7);
		if(currNode != null)
			if(ourPlayer.active2 != null)
				currNode.value = ourPlayer.active2.id*10;//Item 2 ID
			else
				currNode.value = 0;
		
		currNode = getNode(nodeLayerInput, 8);
		if(currNode != null)
			currNode.value = ourPlayer.active1CooldownCurrent;//Item 1 CD
		
		currNode = getNode(nodeLayerInput, 9);
		if(currNode != null)
			currNode.value = ourPlayer.active2CooldownCurrent;//Item 2 CD
		
		//direction
		currNode = getNode(nodeLayerInput, 10);
		if(currNode != null)
			currNode.value = ourPlayer.direction;
		
		//passives here
		
		//jump cd
		currNode = getNode(nodeLayerInput, 11);
		if(currNode != null)
			currNode.value = ourPlayer.extraJumpsCurrent;
		
		//desired position
		currNode = getNode(nodeLayerInput, 12);
		if(currNode != null)
			currNode.value = desiredX;
		currNode = getNode(nodeLayerInput, 13);
		if(currNode != null)
			currNode.value = desiredY;
		
		//score for distance from target
		
		if(Math.Abs(desiredX - ourPlayer.transform.position.x) + Math.Abs(desiredY - ourPlayer.transform.position.y) > 2)
			ticksToDestination++;
		else
			ticksToDestination = 0;
		
		double scale = 5 - ticksToDestination/20;
		
		score += scale*Math.Pow((36 - (desiredX - ourPlayer.transform.position.x))/16,2)/2;
		score += scale*Math.Pow((24 - (desiredY - ourPlayer.transform.position.y))/4,2)/2;
	}
	
	void makeDecisions()
	{
		if(ourPlayer == null)
			return;
		
		AiBase playerA = null;
		AiBase playerB = null;
		AiBase playerC = null;
		
		if(aiBroodmother.ai1 != this && aiBroodmother.ai1 != null)
		{
			if(playerA == null)
				playerA = aiBroodmother.ai1;
			else if(playerB == null)
				playerB = aiBroodmother.ai1;
			else
				playerC = aiBroodmother.ai1;
		}
		if(aiBroodmother.ai2 != this && aiBroodmother.ai2 != null)
		{
			if(playerA == null)
				playerA = aiBroodmother.ai2;
			else if(playerB == null)
				playerB = aiBroodmother.ai2;
			else
				playerC = aiBroodmother.ai2;
		}
		if(aiBroodmother.ai3 != this && aiBroodmother.ai3 != null)
		{
			if(playerA == null)
				playerA = aiBroodmother.ai3;
			else if(playerB == null)
				playerB = aiBroodmother.ai3;
			else
				playerC = aiBroodmother.ai3;
		}
		if(aiBroodmother.ai4 != this && aiBroodmother.ai4 != null)
		{
			if(playerA == null)
				playerA = aiBroodmother.ai4;
			else if(playerB == null)
				playerB = aiBroodmother.ai4;
			else
				playerC = aiBroodmother.ai4;
		}
		
		ourPlayer.aiAttack = false;
		ourPlayer.aiSpecial = false;
		
		//character specific attack protocols
		if(ourPlayer.id == 1)//hero
		{
			//check if enemy is close enough to melee
			double meleeRadius = 2.0;//adjust
		
			if(playerA != null && playerA.ourPlayer != null)
			{
				double dist = Math.Pow(playerA.ourPlayer.transform.position.x - ourPlayer.transform.position.x,2) + Math.Pow(playerA.ourPlayer.transform.position.y - ourPlayer.transform.position.y,2);
				
				if(dist <= meleeRadius)
					ourPlayer.aiAttack = true;
			}
			if(playerB != null && playerB.ourPlayer != null)
			{
				double dist = Math.Pow(playerB.ourPlayer.transform.position.x - ourPlayer.transform.position.x,2) + Math.Pow(playerB.ourPlayer.transform.position.y - ourPlayer.transform.position.y,2);
				
				if(dist <= meleeRadius)
					ourPlayer.aiAttack = true;
			}
			if(playerC != null && playerC.ourPlayer != null)
			{
				double dist = Math.Pow(playerC.ourPlayer.transform.position.x - ourPlayer.transform.position.x,2) + Math.Pow(playerC.ourPlayer.transform.position.y - ourPlayer.transform.position.y,2);
				
				if(dist <= meleeRadius)
					ourPlayer.aiAttack = true;
			}
			
			//if we didn't use a melee, use a ranged if they're within a distance and range
			if(!ourPlayer.aiAttack)
			{
				double rangedWidth = 6.0;//adjust
				double rangedHeight = 1.5;//adjust
				
				
				if(playerA != null && playerA.ourPlayer != null)
				{
					
					if(Math.Pow(playerA.ourPlayer.transform.position.x - ourPlayer.transform.position.x,2) <= rangedWidth && Math.Pow(playerA.ourPlayer.transform.position.y - ourPlayer.transform.position.y,2) <= rangedHeight)
					ourPlayer.aiSpecial = true;
				}
				if(playerB != null && playerB.ourPlayer != null)
				{
					
					if(Math.Pow(playerB.ourPlayer.transform.position.x - ourPlayer.transform.position.x,2) <= rangedWidth && Math.Pow(playerB.ourPlayer.transform.position.y - ourPlayer.transform.position.y,2) <= rangedHeight)
					ourPlayer.aiSpecial = true;
				}
				if(playerC != null && playerC.ourPlayer != null)
				{
					
					if(Math.Pow(playerC.ourPlayer.transform.position.x - ourPlayer.transform.position.x,2) <= rangedWidth && Math.Pow(playerC.ourPlayer.transform.position.y - ourPlayer.transform.position.y,2) <= rangedHeight)
					ourPlayer.aiSpecial = true;
				}
				
				if(ourPlayer.aiSpecial && ourPlayer.specialChargeTime >= ourPlayer.specialChargeTimeMax)
				{
					ourPlayer.aiSpecial = false;
				}
			}
			
		}
		
		//determine where we want to go
		//find highest danger rating
		double ourRating = rateDanger(this);
		
		AiBase highestDanger = this;
		double highestDangerRating = ourRating;
		
		//find lowest danger rating
		AiBase lowestDanger = this;
		double lowestDangerRating = ourRating;
		
		//now check all other players to see whom the highest and lowest is
		double tempRating = ourRating;
		
		int living = 1;
		
		if(playerA != null)
		{
			tempRating = rateDanger(playerA);
			
			if(tempRating > highestDangerRating)
			{
				highestDanger = playerA;
				highestDangerRating = tempRating;
			}
			else if(tempRating < lowestDangerRating)
			{
				lowestDanger = playerA;
				lowestDangerRating = tempRating;
			}
			//default to first AI found to be strongest
			else
			{
				highestDanger = playerA;
				highestDangerRating = tempRating;
			}
			
			if(playerA.ourPlayer != null)
				if(playerA.ourPlayer.currentHealth > 0)
					living++;
		}
		if(playerB != null)
		{
			tempRating = rateDanger(playerB);
			
			if(tempRating > highestDangerRating)
			{
				highestDanger = playerB;
				highestDangerRating = tempRating;
			}
			else if(tempRating < lowestDangerRating)
			{
				lowestDanger = playerB;
				lowestDangerRating = tempRating;
			}
			
			if(playerB.ourPlayer != null)
				if(playerB.ourPlayer.currentHealth > 0)
					living++;
		}
		if(playerC != null)
		{
			tempRating = rateDanger(playerC);
			
			if(tempRating > highestDangerRating)
			{
				highestDanger = playerC;
				highestDangerRating = tempRating;
			}
			else if(tempRating < lowestDangerRating)
			{
				lowestDanger = playerC;
				lowestDangerRating = tempRating;
			}
			//default to last AI found to be weakest
			else
			{
				lowestDanger = playerC;
				lowestDangerRating = tempRating;
			}
			
			if(playerC.ourPlayer != null)
				if(playerC.ourPlayer.currentHealth > 0)
					living++;
		}
		
		double oldDesiredX = desiredX;
		double oldDesiredY = desiredY;
		
		if(lowestDanger.ourPlayer != null && highestDanger.ourPlayer != null)
		{
			//now check if the we are not the weakest for aggressive behavior
			if(ourRating  >= highestDangerRating / 2 || lowestDanger != this)
			{
				//pick on the weakest
				desiredX = lowestDanger.ourPlayer.transform.position.x;
				desiredY = lowestDanger.ourPlayer.transform.position.y;
			}
			//if just us, fite em
			else if(living == 2)
			{
				//believe in me who believes in you (ur gona get rekt son)
				desiredX = highestDanger.ourPlayer.transform.position.x;
				desiredY = highestDanger.ourPlayer.transform.position.y;
			}
			//now check if the we are the lowest for evasive behavior, try to be at the opposite position of our aggressor
			else if(lowestDanger == this)
			{
				//evasive maneuver alpha gamma
				desiredX = 36 - highestDanger.ourPlayer.transform.position.x;
				desiredY = 24 - highestDanger.ourPlayer.transform.position.y;
			}
		}
				
		//reset timer if far enough of a new desired position
		if(Math.Pow(oldDesiredX - desiredX,2) + Math.Pow(oldDesiredY - desiredY,2) > 2)
			ticksToDestination=0;
		
		
		if(ourPlayer.active1 == null)
			ourPlayer.aiPickup1 = true;
		else if(ourPlayer.active2 == null)
			ourPlayer.aiPickup2 = true;
	}
	
	void readOutputs()
	{
		if(ourPlayer == null)
			return;//don't run without a player
		
		Node currNode = null;
		Node currNode2 = null;
		
		//move left or right
		currNode = getNode(nodeLayerOutput, 6);
		currNode2 = getNode(nodeLayerOutput, 1);
		if(currNode != null && currNode2 != null)
		{
			if(currNode.value > currNode2.value)
				ourPlayer.aiDirection = 0;
			else if(currNode.value < currNode2.value)
				ourPlayer.aiDirection = 1;
			else
				ourPlayer.aiDirection = 0.5;
		}
		
		//get our jump
		currNode = getNode(nodeLayerOutput, 2);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiJump = true;
				score+=0.05;//give reward for jumping (will be outweighed by jumping badly in many positions)
			}
			else
			{
				ourPlayer.aiJump = false;
			}
		}
		
		//items
		currNode = getNode(nodeLayerOutput, 3);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiItem1 = true;
				if(ourPlayer.active1CooldownCurrent > 0)
					score-=0.2;//don't use while on cooldown!
				else
					score-=0.1;//reward item usage(slightly)
			}
			else
			{
				ourPlayer.aiItem1 = false;
				score+=0.1;//give reward for saving items
			}
		}
		currNode = getNode(nodeLayerOutput, 4);
		if(currNode != null)
		{
			if(currNode.value > 0.5)
			{
				ourPlayer.aiItem2 = true;
				if(ourPlayer.active2CooldownCurrent > 0)
					score-=0.2;//don't use while on cooldown!
				else
					score-=0.1;//reward item usage(slightly)
			}
			else
			{
				ourPlayer.aiItem2 = false;
				score+=0.1;//give reward for saving items
			}
		}
	}
	
	Node getNode(Node head, int index)
	{
		Node iterator = head;
		
		while(iterator!= null && index > 1)
		{
			iterator = iterator.next;
			index--;
		}
		
		return iterator;
	}
	
	//calculates our score metric based on parameters
	void updateScore()
	{
		
	}
	
	//used for setting the value of an input as they change
	void setInput(int index)
	{
		
	}
	
	//read the output stored for updates every tick
	void getOutput(int index)
	{
		
	}	

	double squashFunction(double input)
	{
		return (1/(1+(Math.Pow(2.718281828,-input))));
	}
	
	public void preload(int input, int layer1, int layer2, int output)
	{
		inputLayerCount = input;
		layer1Count = layer1;
		layer2Count = layer2;
		outputLayerCount = output;	
	}
}