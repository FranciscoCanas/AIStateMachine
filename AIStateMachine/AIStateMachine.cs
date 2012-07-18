using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AIStateMachineSpace
{
    public class AIStateMachine
    {
        /**
         * Members
         **/

        /**
         * Name of the machine.
         **/
        private String machineName;

        /**
         * Name of the currently selected state.
         **/
        private AIState currentState;

        /**
         * List of enum names of all states.
         * */
        private List<AIState> StateNames;

        /**
         * Number of states. ie. Dimension of nxn transition matrix.
         **/
        private int NumStates;

        /**
         * n by n matrix containing the probability vectors for every state transition:
         * Row i holds the probability vector for the transition from state i to states 1 to n.
         * Probabily of 0.0 means never transitions, probability of 1.0 means always transitions.
         **/
        private double[][] TransitionMatrix;

        /**
         * Pseudo-random generator used in transitions.
         **/
        private Random rtransition;

        /**
         * Constructor for txt-initialized AISMs
         **/
        public AIStateMachine(String aifile, String aiName)
        {
            /** Read state names and vectors **/
            //Initialize(numStates, stateNames, tmatrix)
        }

        /**
         * Constructor for manually-initialized AISMs
         **/
        public AIStateMachine(String aiName, int numStates, AIState[] stateNames, double[][] tmatrix)
        {
            machineName = aiName;
            rtransition = new Random();
            StateNames = new List<AIState>(numStates);
            TransitionMatrix = new Double[numStates][];
            Initialize(numStates, stateNames, tmatrix);
        }

        /**
         * Takes a  containing a vectors table and
         * initializes internal state transition table.
         **/
        private void Initialize(int numStates, AIState[] stateNames, double[][] tmatrix)
        {
            NumStates = numStates;
            for (int index = 0; index < numStates; index++)
            {
                StateNames.Add(stateNames[index]);
                TransitionMatrix[index] = new Double[numStates];
                TransitionMatrix[index] = tmatrix[index];
            }

            currentState = StateNames[0];


        }

        /**
         * Returns name of machine.
         **/
        public String GetName()
        {
            return machineName;
        }

        /**
         * Returns the number of states in this machine.
         **/
        public int GetNumStates()
        {
            return NumStates;
        }

        /**
         * Returns the probability vector for the specified state.
         **/
        public double[] GetPVector(AIState state)
        {
            return TransitionMatrix[StateNames.IndexOf(state)];
        }

        /**
         * True if the probabilities in pVector add
         * up to exactly 1.0. False otherwise.
         **/
        static public bool ValidVector(double[] pVector)
        {
            double total = 0.0;

            foreach (Double prob in pVector)
            {
                total += prob;
            }

            return (total == 1.0);
        }


        /**
         * Returns a string with the name/enum of the next state to transition to.
         **/
        public AIState GetNextState()
        {
            int index = 0;
            double[] PVector = GetPVector(currentState);
            double newdraw = rtransition.NextDouble();
            double cdnum = PVector[0];


            /**
             * All PVectors contain values which add up to 1.0.
             * This loop iterates through them, accumulating these values 
             * until random number newdraw is less than the accumulated value 
             * (cdnum: cumulative distribution number).
             * [0.5,0.5]
             * newdraw = 0.25
             * first iter: 
             * When we reach this, index gives us the new state to transition to.
             **/
            while (cdnum < newdraw)
            {
                index++;
                cdnum += PVector[index];
                
            }


            currentState = StateNames[index]; 
            return StateNames[index];
        }

        /**
         * Gets the currently active state of the machine.
         **/
        public AIState GetCurrentState()
        {
            return currentState;
        }

        /**
         * Forces transition to the specified state.
         **/
        public void SetCurrentState(AIState state)
        {
            currentState = state;
        }

        /**
         * Returns the list of state names this machine uses
         **/
        public List<AIState> GetStateNames()
        {
            return StateNames;
        }

        /** 
         * Adds a new state to the machine and updates the transition matrix.
         * pVectorTo specifies the probability transitions from current
         * existing states to new state.
         * pVectorFrom specifies the probability transitions from new state
         * to the currently existing states.
         **/
        public void AddState(AIState newState, Double[] pVectorTo, Double[] pVectorFrom)
        {

        }

        /**
         * Removes a state from the machine and updates transition matrix.
         * TODO: Checks to make sure we're removing an existing state.
         * TODO: If we are removing CURRENT state, then throw warning and transition.
         * TODO: RemoveState and AddState need to take new matrices as input, 
         * to uphold 1.0 property below:
         * TODO: All new pvectors must be validated (ie. add up to 1.0). Write validation helper.
         **/
        public void RemoveState(AIState state)
        {
            int removeIndex = StateNames.IndexOf(state);
            int newVectorIndex = 0;
            int newProbIndex = 0;

            double[][] newTransitionMatrix = new double[NumStates - 1][];

            /** 
             * If removing currentState, or only one state left.
             **/
            if (state.Equals(currentState) || (NumStates == 1) || !(StateNames.Contains(state)))
            {
                return;
            }

            /**
             * For every row vector in the transitional matrix:
             **/
            for (int vectorIndex = 0; vectorIndex < NumStates; vectorIndex++)
            {
                // If it doesn't correspond to the state we are removing:
                if (vectorIndex != removeIndex)
                {
                    newProbIndex = 0;
                    newTransitionMatrix[newVectorIndex] = new Double[NumStates - 1];
                    
                    /**
                     * Then for every probability in this vector:
                     **/
                    for (int probIndex = 0; probIndex < NumStates; probIndex++)
                    {
                        
                        // If it doesn't correspond to the state we are removing:
                        if (probIndex != removeIndex)
                        {
                            /**
                             * Then copy it into the new Transitional Matrix
                             * adjusting value for the removed state so as to
                             * still uphold the transition matrix property of
                             * every vector adding up to 1.0
                             **/
                            newTransitionMatrix[newVectorIndex][newProbIndex] = 
                                TransitionMatrix[vectorIndex][probIndex] +
                                TransitionMatrix[vectorIndex][removeIndex] / (NumStates - 1);

                            newProbIndex++;
                        }
                        
                    }
                    newVectorIndex++;
                }

                
            }

            TransitionMatrix = newTransitionMatrix;
            NumStates -= 1;
            StateNames.Remove(state);


        }


    }
}
