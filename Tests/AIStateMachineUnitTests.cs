using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIStateMachineSpace;

namespace Tests
{
    class AIStateMachineUnitTests
    {
        const String SEP = "============================================================================";
        const String FAIL = "FAIL";
        const String PASS = "PASS";

        AIStateMachine m1;
        AIStateMachine m2;
        AIStateMachine m3;

        public AIStateMachineUnitTests() {

            AIState[] m1Names = { AIState.Chase, AIState.Avoid };
            Double[][] m1TMatrix = {
                        new Double[2] {0.5, 0.5},
                        new Double[2] {0.5, 0.5}
                                   };


            AIState[] m2Names = { AIState.Chase, AIState.Avoid, AIState.Hide };
            Double[][] m2TMatrix = {
                        new Double[3] {0.25, 0.5, 0.25},
                        new Double[3] {0.25, 0.25, 0.5},
                        new Double[3] {0.5, 0.25, 0.25}
                                   };

            AIState[] m3Names = { AIState.Chase, AIState.Avoid, AIState.Hide, AIState.Shoot };
            Double[][] m3TMatrix = {
                        new Double[4] {0.1, 0.5, 0.2, 0.2},
                        new Double[4] {0.2, 0.1, 0.5, 0.2},
                        new Double[4] {0.2, 0.2, 0.1, 0.5},
                        new Double[4] {0.5, 0.2, 0.2, 0.1}
                                   };


    


            /**
             * Instantiate test machines
             **/
            Console.WriteLine("Initializing Machine 1...");
            m1 = new AIStateMachine(2, m1Names, m1TMatrix);
            Console.WriteLine("Success.");
            Console.WriteLine("Initializing Machine 2...");
            m2 = new AIStateMachine(3, m2Names, m2TMatrix);
            Console.WriteLine("Success.");
            Console.WriteLine("Initializing Machine 3...");
            m3 = new AIStateMachine(4, m3Names, m3TMatrix);
            Console.WriteLine("Success.");

            /**
             * Run Tests
             **/
            
            Console.WriteLine(TestMembers(m1));
            Console.WriteLine(TestMembers(m2));
            Console.WriteLine(TestMembers(m3));

            Console.WriteLine(TestGetNextState(m1, 4));
            Console.WriteLine(TestGetNextState(m2, 4));
            Console.WriteLine(TestGetNextState(m3, 16));



        }

        public String TestMembers(AIStateMachine current) {

            String result = FAIL;
            Console.WriteLine(SEP);
            Console.WriteLine("Testing Machine Members:");

            Console.WriteLine("1) Machine State Names:");
            try
            {
                foreach (AIState state in current.GetStateNames())
                {
                    Console.WriteLine(state.ToString());
                }
            }
            catch (SystemException)
            {
                Console.Write("Machine State Names Test ");
                return FAIL;
            }


            Console.WriteLine("2) Machine Current State:");
            Console.WriteLine(current.GetCurrentState().ToString());

            Console.WriteLine("2) Machine State Vectors:");

            try
            {
                foreach (AIState state in current.GetStateNames())
                {
                    Double[] pvector = new Double[current.GetStateNames().Count()];
                    pvector = current.GetPVector(state);
                    Console.Write(state.ToString());
                    Console.Write(": [");
                    for (int pIndex = 0; pIndex < current.GetStateNames().Count(); pIndex++)
                    {
                        Console.Write(pvector[pIndex]);
                        Console.Write(" ");
                    }
                    Console.WriteLine("]");
                }
            }
            catch (SystemException)
            {
                Console.Write("Machine PVector Test ");
                return FAIL;
            }



            result = PASS;
            return result;
        }

        public String TestGetNextState(AIStateMachine current, int limit)
        {
            String result = PASS;
            Console.WriteLine(SEP);
            Console.WriteLine("Testing Get Next State:");
            try
            {
                for (int transition = 0; transition < limit; transition++)
                {
                    Console.WriteLine(current.GetNextState().ToString());
                }
            }
            catch (SystemException)
            {
                Console.Write("Machine GetNextState Test ");
                return FAIL;
            }

           
            return result;
        }
       
    }
}
