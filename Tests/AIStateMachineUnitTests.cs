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
            m1 = new AIStateMachine("m1", 2, m1Names, m1TMatrix);
            Console.WriteLine("Success.");
            Console.WriteLine("Initializing Machine 2...");
            m2 = new AIStateMachine("m2", 3, m2Names, m2TMatrix);
            Console.WriteLine("Success.");
            Console.WriteLine("Initializing Machine 3...");
            m3 = new AIStateMachine("m3", 4, m3Names, m3TMatrix);
            Console.WriteLine("Success.");

            /**
             * Run Tests
             **/

            Console.WriteLine(TestValidVector());
            
            Console.WriteLine(TestMembers(m1));
            Console.WriteLine(TestMembers(m2));
            Console.WriteLine(TestMembers(m3));

            Console.WriteLine(TestGetNextState(m1, 4));
            Console.WriteLine(TestGetNextState(m2, 4));
            Console.WriteLine(TestGetNextState(m3, 16));

            Console.WriteLine(TestRemoveState(m1, AIState.Chase));
            Console.WriteLine(TestMembers(m1));

            Console.WriteLine(TestRemoveState(m2,AIState.Hide));
            Console.WriteLine(TestMembers(m2));

            Console.WriteLine(TestRemoveState(m3, AIState.Hide));
            Console.WriteLine(TestMembers(m3));
            Console.WriteLine(TestGetNextState(m3, 32));

            Console.WriteLine(TestFixPVector());



        }

        public String TestValidVector()
        {
            String result = PASS;
            Console.WriteLine("Testing ValidVector:");

            if (!AIStateMachine.ValidVector(new double[] { 1.0 }))
            {
                Console.WriteLine("{1.0}");
                return FAIL;
            }

            if (!AIStateMachine.ValidVector(new double[] {0.5, 0.5}))
            {
                Console.WriteLine("{0.5, 0.5}");
                return FAIL;
            }

            if (AIStateMachine.ValidVector(new double[] { 0.5, 0.5, 0.75 }))
            {
                Console.WriteLine("{0.5, 0.5, 0.75}");
                return FAIL;
            }

            if (!AIStateMachine.ValidVector(new double[] { 0.5, 0.5, 0.0 }))
            {
                Console.WriteLine("{0.5, 0.5, 0.0}");
                return FAIL;
            }

            return result;

        }

        public String TestFixPVector()
        {
            double[] pv1;
            double[] pv2;
            double[] pv3;

            String result = PASS;
            Console.WriteLine(SEP);
            Console.WriteLine("Testing FixPVector:");
            
            try
            {
                pv1 = AIStateMachine.FixPVector(new double[] { 0.3, 0.3 });
                pv2 = AIStateMachine.FixPVector(new double[] { 0.6, 0.6 });
                pv3 = AIStateMachine.FixPVector(new double[] { 0.1, 0.2, 0.1 });

                
            }
            catch (SystemException exc)
            {
                Console.WriteLine(exc.ToString());
                Console.Write("FixPVector tests ");
                return FAIL;
            }

            PrintProbVector(pv1);
            PrintProbVector(pv2);
            PrintProbVector(pv3);

            return result;
        }

        public void PrintProbVector(double[] vector)
        {
            Console.Write("[ ");
            foreach (double prob in vector)
            {
                Console.Write(prob + " ");
            }
            Console.WriteLine("]");
        }

        public String TestMembers(AIStateMachine current) {

            String result = FAIL;
            Console.WriteLine(SEP);
            Console.Write("Testing Machine Members for ");
            Console.WriteLine(current.GetName());

            Console.WriteLine("1) Machine State Names:");
            try
            {
                foreach (AIState state in current.GetStateNames())
                {
                    Console.WriteLine(state.ToString());
                }
            }
            catch (SystemException exc)
            {
                Console.WriteLine(exc.ToString());
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
                    Console.Write(state.ToString() + ": ");
                    PrintProbVector(pvector);
                  
                }
            }
            catch (SystemException exc)
            {
                Console.WriteLine(exc.ToString());
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
            Console.Write("Testing Get Next State ");
            Console.WriteLine(current.GetName());
            try
            {
                for (int transition = 0; transition < limit; transition++)
                {
                    Console.WriteLine(current.GetNextState().ToString());
                }
            }
            catch (SystemException exc)
            {
                Console.WriteLine(exc.ToString());
                Console.Write("Machine GetNextState Test ");
                return FAIL;
            }

           
            return result;
        }

        public String TestRemoveState(AIStateMachine current, AIState state) {
            String result = PASS;
            int oldNum;

            Console.WriteLine(SEP);
            Console.WriteLine("Testing RemoveState " + current.GetName() + " " + state.ToString());
            

            try
            {
                oldNum = current.GetNumStates();
                current.RemoveState(state);
                if (current.GetNumStates() != (oldNum - 1))
                {
                    result = FAIL;
                }
            }
            catch (SystemException exc)
            {
                Console.WriteLine(exc.ToString());
                Console.WriteLine("Machine Remove State Test ");
                return FAIL;
            }

            foreach (AIState newState in current.GetStateNames())
            {
                Console.WriteLine(newState.ToString() + "P Vector Test: " 
                    + AIStateMachine.ValidVector(current.GetPVector(newState)));
            }


            return result;

        }
       
    }
}
