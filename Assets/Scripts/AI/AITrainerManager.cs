using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class AiTrainerManager
{
	//each of the AI we are responsible for
	//we load in data for the four of them to train
	//to reset, simply replace the data
	public AiBase ai1 = null;
	public AiBase ai2 = null;
	public AiBase ai3 = null;
	public AiBase ai4 = null;
	
	public bool trainingMode = false;
	public bool runningMode = true;
	public bool disabled = false;
	
	bool started = false;
	
	bool reset = false;
	
	public GameManager gameManager = null;
	
	//this is used to denote what stage of the mode is active
	public string externalStage = "Training";
	int currentGenome = 1;
	const int maxGenome = 64;
	int currentSet = 1;
	int totalIterations = 1;
	
	int genomesPerSet = 4;
	
	const double timerReset = 20;
	
	double lastTime = timerReset;
	double timer = 0;//timerReset seconds per cycle default
	
	double[] scores = new double[maxGenome];
	
	List<double>[] inputToLayer1Genes = new List<double>[maxGenome];
	List<double>[] layer1ToLayer2Genes = new List<double>[maxGenome];
	List<double>[] layer2ToOutputGenes = new List<double>[maxGenome];
	
	//breeding & mutation stuff
	const int fittestNum = maxGenome / 4;//16
	const int randomGenomes = maxGenome / 8;//8
	const int highMutationNum = maxGenome / 4;//16
	const int mediumMutationNum = maxGenome / 8;//8
	const int lowMutationNum = maxGenome / 4;//16
	
	int[] fittestIDs = new int[fittestNum];
	
	//for our reference
	int inputLayerCount = 14;
	int layer1Count = 25;
	int layer2Count = 20;
	int outputLayerCount = 6;
	
	double mutationAmount = 0.005;//0.01, 0.1, 1.0 for mutation amounts
	
	//determine how much of the genome will be mutated
	double mutationPercentLower = 0.01;//1%
	double mutationPercentUpper = 0.02;//2%
	
	public System.Random rnd = new System.Random();

	// Use this for initialization
	public void Start ()
	{
		disabled = !trainingMode && !runningMode;
		
		if(disabled)
			return;
		
		if(trainingMode)//pump the timescale if we're training to learn faster!
		{
			Time.timeScale = 6.0f;
		}
		
		//create the genome storage
		for(int i=0; i<maxGenome; i++)
		{
			inputToLayer1Genes[i] = new List<double>();
			layer1ToLayer2Genes[i] = new List<double>();
			layer2ToOutputGenes[i] = new List<double>();
		}
		
		//string text = System.IO.File.ReadAllText("myfile.txt");
		//System.IO.File.WriteAllText("AI/weights_", "7 microhitlers\ntesting");
		
		//generate the data (random for now) for all failures to load
		for(int i=0; i<maxGenome; i++)
		{
			createRandomGenome(i);
		}
		
		int index;
		
		//load our files
		for(index=0; index<maxGenome && !reset; index++)
		{
			string fileData = System.IO.File.ReadAllText("AI/weights_"+(index+1)+".dat".ToString());
			
			//halt if we failed to load it
			if(fileData.Length <= 5)
				break;
			
			//load in the data
			
			string[] set = fileData.Split(new char[]{'|'});
			
			string[] set1 = set[0].Split(new char[]{' '});
			string[] set2 = set[1].Split(new char[]{' '});
			string[] set3 = set[2].Split(new char[]{' '});
			
			//load in ALL of the values
			for(int i=0; i<set1.Length - 1; i++)
				inputToLayer1Genes[index][i] = Convert.ToDouble(set1[i]);
			for(int i=0; i<set2.Length - 1; i++)
				layer1ToLayer2Genes[index][i] = Convert.ToDouble(set2[i]);
			for(int i=0; i<set3.Length - 1; i++)
				layer2ToOutputGenes[index][i] = Convert.ToDouble(set3[i]);
		}
		
		Debug.Log("Loaded up to index "+index.ToString());
		
		//shuffle our pool here
		shuffleGeneticPool();
	}
	
	public void OnDestroy()
	{
		
		if(disabled)
			return;
		//store our genomes quickly!
		
		
		//destroy the genome storage
		for(int i=0; i<maxGenome; i++)
		{
			inputToLayer1Genes[i].Clear();
			layer1ToLayer2Genes[i].Clear();
			layer2ToOutputGenes[i].Clear();
		}
	}
	
	// Update is called once per frame
	public void Update ()
	{
		if(disabled)
			return;
		
		//get the match state
		//do not tick if there is no match
		if(gameManager.stage != null && !gameManager.end)
		{
			timer -= Time.deltaTime;
			
			if(lastTime > Math.Floor(timer))
			{
				Debug.Log("AI Timer: "+(lastTime.ToString())+" of set "+(currentSet.ToString())+". Iteration "+(totalIterations.ToString()));
				lastTime = Math.Floor(timer);
			}
			
			//check if the AI isn't learning anything
			//if so, end prematurely by 5 seconds
			if(trainingMode && timer <= timerReset - 5)
			{
				//total the score of all AI
				double totalScore = 0;
				
				if(ai1 != null)
					totalScore += Math.Max(ai1.score,0);
				
				if(ai2 != null)
					totalScore += Math.Max(ai2.score,0);
				
				if(ai3 != null)
					totalScore += Math.Max(ai3.score,0);
				
				if(ai4 != null)
					totalScore += Math.Max(ai4.score,0);
				
				//threshold it?
				if(totalScore < 0)
				{
					Debug.Log("Premature Ending!");
					timer = -1;
				}
			}
			
			//check if the timer is up
			if(timer < 0 && trainingMode)
			{
				if(ai1 != null)
				{
					//score the prior genome, if any
					scores[(currentSet - 1)*genomesPerSet] = ai1.score;
				}
				if(ai2 != null)
				{
					scores[(currentSet - 1)*genomesPerSet + 1] = ai2.score;
				}
				if(ai3 != null)
				{
					scores[(currentSet - 1)*genomesPerSet + 2] = ai3.score;
				}
				if(ai4 != null)
				{
					scores[(currentSet - 1)*genomesPerSet + 3] = ai4.score;
				}
					
				gameManager.end = true;//we force the match to end
				
				//reset our timer
				timer = timerReset;
				lastTime = timerReset;
				
				
				if(started)
				{
					//make sure to step our genetic process here!
					currentSet++;
					
					if(currentSet == 17)
						Debug.Log("RESET ME");
					
					if((currentSet)*genomesPerSet  > maxGenome)
					{				
						//reset our stats
						currentSet = 1;
						totalIterations++;
						
						//ignore modifying genomes if not training
						if(trainingMode)
						{
							//grab best
							selectFittest();
							
							//mix genetics here
							modifyGenetics();
			
							//shuffle our pool here
							shuffleGeneticPool();
						}
						
						//reset the scorings
						for(int i=0; i<maxGenome; i++)
							scores[i] = 0.0;
					}
					
				}
				else
				{
					//reset the scorings
					for(int i=0; i<maxGenome; i++)
						scores[i] = 0.0;
					
					started = true;
				}
			}
		}
	}
	
	public void modifyGenetics()
	{	
		//save out our prior generation!
		for(int i=0; i<maxGenome; i++)
		{
			string dataOutput = "";
			
			for(int j=0; j<inputLayerCount*layer1Count; j++)
			{
				dataOutput+= inputToLayer1Genes[i][j].ToString() + " ";
			}
			dataOutput+="|\n";
			
			for(int j=0; j<layer2Count*layer1Count; j++)
			{
				dataOutput+= layer1ToLayer2Genes[i][j].ToString() + " ";
			}
			dataOutput+="|\n";
			
			for(int j=0; j<outputLayerCount*layer2Count; j++)
			{
				dataOutput+= layer2ToOutputGenes[i][j].ToString() + " ";
			}
			dataOutput+="|\n";
			
			System.IO.File.WriteAllText("AI/weights_"+(i+1)+".dat".ToString(), dataOutput);
		}
	
		//move fittest 8 to top 8 slots
		for(int i=0; i<fittestNum; i++)
		{
			swapGenes(inputToLayer1Genes[i], inputToLayer1Genes[fittestIDs[i]]);
			swapGenes(layer1ToLayer2Genes[i], layer1ToLayer2Genes[fittestIDs[i]]);
			swapGenes(layer2ToOutputGenes[i], layer2ToOutputGenes[fittestIDs[i]]);
		}
		
		//now clear slots 8 to 15 and put in new values
		for(int i=fittestNum; i<fittestNum + randomGenomes; i++)
		{
			clearGenome(i);
			createRandomGenome(i);
		}
		
		//pick random parents for everywhere
		for(int i=fittestNum+randomGenomes; i<maxGenome; i++)
		{
			clearGenome(i);
			
			//pick one of our fabled 8 to copy to it
			int parent = rnd.Next(0, fittestNum);
			
			//copy them genes
			inputToLayer1Genes[i].AddRange(inputToLayer1Genes[parent]);
			layer1ToLayer2Genes[i].AddRange(layer1ToLayer2Genes[parent]);
			layer2ToOutputGenes[i].AddRange(layer2ToOutputGenes[parent]);
		}
		
		//now lets make our 16 high-mutation-rate genomes
		for(int i=fittestNum+randomGenomes; i<fittestNum + randomGenomes + highMutationNum; i++)
		{			
		
			double mutationChance = rnd.Next((int)mutationPercentLower*1000000, (int)mutationPercentUpper*1000000)/1000000.0;
			
			
			//mutate the network!
			//mutate across ALL of the genes
			for(int j=0; j<inputLayerCount*layer1Count; j++)
			{
				double mutateRate = (inputToLayer1Genes[i][j] * 0.1)-inputToLayer1Genes[i][j]*0.05;
				mutate((float)mutateRate, mutationChance, inputToLayer1Genes[i], j);
			}
			for(int j=0; j<layer1Count*layer2Count; j++)
			{
				double mutateRate = (layer1ToLayer2Genes[i][j] * 0.1)-layer1ToLayer2Genes[i][j]*0.05;
				mutate((float)mutateRate, mutationChance, layer1ToLayer2Genes[i], j);
			}
			for(int j=0; j<layer2Count*outputLayerCount; j++)
			{
				double mutateRate = (layer2ToOutputGenes[i][j] * 0.1)-layer2ToOutputGenes[i][j]*0.05;
				mutate((float)mutateRate, mutationChance, layer2ToOutputGenes[i], j);
			}
		}
		
		//now lets make our 16 med-mutation-rate genomes
		for(int i=fittestNum + randomGenomes + highMutationNum; i<fittestNum + randomGenomes + highMutationNum + mediumMutationNum; i++)
		{			
		
			double mutationChance = rnd.Next((int)mutationPercentLower*1000000, (int)mutationPercentUpper*1000000)/1000000.0;
			
			float mutateRate = (float)mutationAmount*100;
			
			//mutate the network!
			//mutate across ALL of the genes
			for(int j=0; j<inputLayerCount*layer1Count; j++)
			{
				mutate(mutateRate, mutationChance, inputToLayer1Genes[i], j);
			}
			for(int j=0; j<layer1Count*layer2Count; j++)
			{
				mutate(mutateRate, mutationChance, layer1ToLayer2Genes[i], j);
			}
			for(int j=0; j<layer2Count*outputLayerCount; j++)
			{
				mutate(mutateRate, mutationChance, layer2ToOutputGenes[i], j);
			}
		}
		
		//now lets make our 16 low-mutation-rate genomes
		for(int i=fittestNum + randomGenomes + highMutationNum + mediumMutationNum; i<fittestNum + randomGenomes + highMutationNum + mediumMutationNum + lowMutationNum && i<maxGenome; i++)
		{			
		
			double mutationChance = rnd.Next((int)mutationPercentLower*1000000, (int)mutationPercentUpper*1000000)/1000000.0;
			
			float mutateRate = (float)mutationAmount;
			
			//mutate the network!
			//mutate across ALL of the genes
			for(int j=0; j<inputLayerCount*layer1Count; j++)
			{
				mutate(mutateRate, mutationChance, inputToLayer1Genes[i], j);
			}
			for(int j=0; j<layer1Count*layer2Count; j++)
			{
				mutate(mutateRate, mutationChance, layer1ToLayer2Genes[i], j);
			}
			for(int j=0; j<layer2Count*outputLayerCount; j++)
			{
				mutate(mutateRate, mutationChance, layer2ToOutputGenes[i], j);
			}
		}
	}
	
	public void mutate(float mutateRate, double mutationChance, List<double> destination, int index)
	{
		double procced = rnd.Next(0,100000)/100000.0;
		
		if(mutationChance > procced)
		{
			//mutate this node
			double amount = rnd.Next((int)-mutateRate*1000000, (int)mutateRate*1000000)/1000000.0;
			
			destination[index] += amount;
		}
	}
	
	public void swapGenes(List<double> a, List<double> b)
	{
		List<double> tempArr = a;
		a = b;
		b = tempArr;
	}
	
	public void clearGenome(int i)
	{
		inputToLayer1Genes[i].Clear();
		layer1ToLayer2Genes[i].Clear();
		layer2ToOutputGenes[i].Clear();
	}
	
	public void createRandomGenome(int i)
	{
		for(int j=0; j<inputLayerCount*layer1Count; j++)
		{
			inputToLayer1Genes[i].Add(rnd.Next(-1000, 1000)/100.0f);
		}
		for(int j=0; j<layer1Count*layer2Count; j++)
		{
			layer1ToLayer2Genes[i].Add(rnd.Next(-1000, 1000)/100.0f);
		}
		for(int j=0; j<layer2Count*outputLayerCount; j++)
		{
			layer2ToOutputGenes[i].Add(rnd.Next(-1000, 1000)/100.0f);
		}
	}
	
	public void selectFittest()
	{
		//default the fittest scores
		for(int i=0; i<fittestNum; i++)
			fittestIDs[i] = -1;
		
		//takes the best 64 fittest ones
		for(int i=0; i<maxGenome; i++)
		{
			bool doneChecking = false;
			for(int j=0; j<fittestNum && !doneChecking; j++)
			{
				if(fittestIDs[j] == -1 || scores[i] > scores[fittestIDs[j]])
				{
					int temp = fittestIDs[j];
					fittestIDs[j] = i;
					
					for(int k=j+1; k<fittestNum; k++)
					{
						int ntemp = fittestIDs[k];
						fittestIDs[k] = temp;
						temp = ntemp;
					}
					
					doneChecking = true;
				}
			}
		}
		
		//show best IDs and scores
		for(int i=0; i<fittestNum; i++)
		{
			Debug.Log("AI "+fittestIDs[i].ToString()+" scored: "+scores[fittestIDs[i]].ToString());
		}
		
		//sort them, for now
		Array.Sort(fittestIDs);
	}
	
	public void loadAI()
	{
		//attempt to load in all of the weights for this set to our precious little AI
		currentGenome = (currentSet - 1)*genomesPerSet + 1;
		if(ai1 != null && currentGenome <= maxGenome)
		{				
			Debug.Log("Loading an AI for Player 1! Genome "+currentGenome.ToString());
			loadNetwork(ai1);
		}
		
		currentGenome = (currentSet - 1)*genomesPerSet + 2;
		if(ai2 != null && currentGenome <= maxGenome)
		{
			Debug.Log("Loading an AI for Player 2! Genome "+currentGenome.ToString());
			loadNetwork(ai2);
		}
		
		currentGenome = (currentSet - 1)*genomesPerSet + 3;
		if(ai3 != null && currentGenome <= maxGenome)
		{
			Debug.Log("Loading an AI for Player 3! Genome "+currentGenome.ToString());
			loadNetwork(ai3);
		}
		
		currentGenome = (currentSet - 1)*genomesPerSet + 4;
		if(ai4 != null && currentGenome <= maxGenome)
		{
			Debug.Log("Loading an AI for Player 4! Genome "+currentGenome.ToString());
			loadNetwork(ai4);
		}
	}
	
	//the shuffle algorithm for shuffling our genetic sets
	public void shuffleGeneticPool()
	{
		for (int i = maxGenome - 1; i > 0; i--) {
			int r = rnd.Next(0,1);
			List<double> tmp1 = inputToLayer1Genes[i];
			List<double> tmp2 = layer1ToLayer2Genes[i];
			List<double> tmp3 = layer2ToOutputGenes[i];
			inputToLayer1Genes[i] = inputToLayer1Genes[r];
			layer1ToLayer2Genes[i] = layer1ToLayer2Genes[r];
			layer2ToOutputGenes[i] = layer2ToOutputGenes[r];
			inputToLayer1Genes[r] = tmp1;
			layer1ToLayer2Genes[r] = tmp2;
			layer2ToOutputGenes[r] = tmp3;
		}
	}
	
	public void linkAI(AiBase playerAI, int index)
	{		
		if(playerAI == null)
			Debug.Log("Clearing AI Slot "+index.ToString());
	
		//don't forget to generate their networks when we do this!
		switch(index)
		{
			case 1:
				ai1 = playerAI;
				break;
			case 2:
				ai2 = playerAI;
				break;
			case 3:
				ai3 = playerAI;
				break;
			case 4:
				ai4 = playerAI;
				break;
		}
		
	
		if (playerAI != null)
			playerAI.aiBroodmother = this;
	}
	
	public void loadNetwork(AiBase ai)
	{
		ai.preload(inputLayerCount, layer1Count, layer2Count, outputLayerCount);
	
		double[] inputLayer1 = new double[inputLayerCount*layer1Count];
		double[] layer1Layer2 = new double[layer2Count*layer1Count];
		double[] layer2Output = new double[layer2Count*outputLayerCount];
		
		inputToLayer1Genes[currentGenome-1].CopyTo( inputLayer1 );
		layer1ToLayer2Genes[currentGenome-1].CopyTo( layer1Layer2 );
		layer2ToOutputGenes[currentGenome-1].CopyTo( layer2Output );
		
		ai.createNetwork(inputLayer1, layer1Layer2, layer2Output);
	}
	
}
