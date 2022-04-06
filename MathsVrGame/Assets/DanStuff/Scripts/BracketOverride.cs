using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EquationHub
{
    public class BracketOverride : MonoBehaviour
    {
        public static BracketOverride instance;
        private void Awake()
        {
            instance = this;
        }

        private EquationManager manager;
        private void Start()
        {
            manager = EquationManager.instance;
        }
        public void NewBidmasOrder()
        {
            if (manager.inBrackets)
            {
                Debug.Log("Bidmas order called from child");

                if (FoundOperator(1005))
                {
                    //Indicies (Square the number)
                    manager.Calculation(0, 2, 0, manager.ReturnIndex(manager.index).Item1, manager.index);
                }
                else if (FoundOperator(1004))
                {
                    //Division 
                    manager.Calculation(0, 0, 0, manager.ReturnIndex(manager.index).Item1, manager.ReturnIndex(manager.index).Item2);
                }
                else if (FoundOperator(1003))
                {
                    //Multiply
                    //Find the next valid numbers 
                    Debug.Log("A index value " + manager.ReturnIndex(manager.index).Item1.ToString());
                    Debug.Log("B index value " + manager.ReturnIndex(manager.index).Item2.ToString());
                    manager.Calculation(0, 0, 0, manager.ReturnIndex(manager.index).Item1, manager.ReturnIndex(manager.index).Item2);
                    Debug.Log("Multiply");
                }
                else if (FoundOperator(1001))
                {
                    //Add
                    //Find the next valid numbers 
                    //Calculation(0, 0, 0, ReturnIndex(index).Item1, ReturnIndex(index).Item2);
                    manager.Calculation(0, 0, 0, manager.ReturnIndex(manager.index).Item1, manager.ReturnIndex(manager.index).Item2);
                    Debug.Log("Add");
                }
                else if (FoundOperator(1002))
                {
                    //Subtraction
                    //Find the next valid numbers 
                    manager.Calculation(0, 0, 0, manager.ReturnIndex(manager.index).Item1, manager.ReturnIndex(manager.index).Item2);
                }

                BracketsComplete();
            }
        }

        private bool FoundOperator(int target)
        {
            for (int i = DetermineLength().Item1; i < DetermineLength().Item2; i++)
            {
                if (manager.numberList[i] == target)
                {
                    manager.index = i;
                    Debug.Log("manager index " + manager.index.ToString());
                    return true;
                }
            }
            return false;
        }

        private int startingPoint = 0;
        private int endingPoint = 0;

        private Tuple<int, int> DetermineLength()
        {
            //int startingPoint = 0;
            //int endingPoint = 0;
            bool bracket1Found = false;
            bool bracket2Found = false;

            for (int i = 0; i < manager.numberList.Count; i++)
            {
                if (manager.numberList[i] == 1006 && !bracket1Found)
                {
                    startingPoint = i;
                    bracket1Found = true;
                }
                else if (manager.numberList[i] == 1006 && !bracket2Found)
                {
                    endingPoint = i;
                    bracket2Found = true;
                }
            }
            //Debug.Log("Starting bracket is here " + startingPoint.ToString());
            //Debug.Log("Ending bracket is here " + endingPoint.ToString());
            return new Tuple<int, int>(startingPoint, endingPoint);
        }

        private bool BracketsComplete()
        {
            int count = 0;
            int start = DetermineLength().Item1;
            int end = DetermineLength().Item2;
            for (int i = start; i < end; i++)
            {
                if (manager.numberList[i] == 0)
                {
                    count++;
                }
            }

            //Check if all the brackets have finished calculating by tracking the amount of zeros 
            if (count == endingPoint - startingPoint - 2)
            {
                Debug.Log("Finished with brackets");
                manager.numberList[startingPoint] = 0;
                Debug.Log(manager.numberList[startingPoint]);
                manager.numberList[endingPoint] = 0;
                manager.inBrackets = false;
                //EquationManager.instance.BidmasOrder();
                return true;
            }
            //Recall this in the event where there is more than one equation in the brackets
            NewBidmasOrder();
            return false;
        }

    }
}

