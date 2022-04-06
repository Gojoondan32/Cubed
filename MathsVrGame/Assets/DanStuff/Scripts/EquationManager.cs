using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace EquationHub
{
    public class EquationManager : MonoBehaviour
    {
        #region Singleton
        public static EquationManager instance;

        private void Awake()
        {
            instance = this;

        }
        #endregion

        public List<int> numberList = new List<int>();
        [SerializeField] private int playerTotal;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Light mainLight;
        public int index;
        public bool inBrackets = false;

        private void Start()
        {
            inBrackets = false;
            //Populate the list at the start with empty values
            for (int i = 0; i < 10; i++)
            {
                numberList.Add(0);
            }
        }

        public void DetermineType(int position, int value)
        {
            if (numberList[position] == 0)
            {
                //Update the list position to equal the value passed in
                numberList[position] = value;
            }
        }

        public void DetermineType(int position)
        {
            numberList[position] = 0;
        }

        private int DetermineOperation(int value, int a, int b)
        {
            switch (value)
            {
                case 1001:
                    return OperationDelegates.instance.Add(a, b);
                case 1002:
                    return OperationDelegates.instance.Subtract(a, b);
                case 1003:
                    return OperationDelegates.instance.Multiply(a, b);
                case 1004:
                    return OperationDelegates.instance.Divide(a, b);
                case 1005:
                    float x = a;
                    float y = b;
                    //Must cast as an integer here because Mathf.Pow only takes / returns float values and this function returns an integer
                    return (int)OperationDelegates.instance.Exponent(x, y);
                default:
                    Debug.Log("Nothing to calculate");
                    return 0;
            }

        }

        public int Calculation(int a = 0, int b = 0, int operationValue = 0, int startingValue = 0, int endingPoint = 0)
        {
            //Set the ending point to the length of the list if the value isn't given
            if (endingPoint == 0)
            {
                endingPoint = numberList.Count;
            }

            for (int i = startingValue; i <= endingPoint; i++)
            {
                //Find the values needed 
                if (numberList[i] < 1000 && a == 0 && numberList[i] != 0)
                {
                    //Block is a number
                    a = numberList[i];
                    numberList[i] = 0;
                    Debug.Log("A's value is: " + a.ToString());
                }
                else if (numberList[i] < 1000 && b == 0 && numberList[i] != 0)
                {
                    b = numberList[i];
                    numberList[i] = 0;
                    Debug.Log("B's value is: " + b.ToString());
                }
                else if (numberList[i] > 1000 && operationValue == 0 && numberList[i] < 1006 && numberList[i] != 0)
                {
                    operationValue = numberList[i];
                    numberList[i] = 0;
                }

                if (a != 0 && b != 0 && operationValue != 0)
                {
                    Debug.Log("operation " + operationValue.ToString());
                    //Perfrom the calculation with the correct operation
                    playerTotal = DetermineOperation(operationValue, a, b);

                    Debug.Log("Total:" + playerTotal.ToString());
                    //Reset all the values to 0
                    numberList[startingValue + 1] = playerTotal;

                    return playerTotal;

                }

            }
            return 0;
        }


        public void BidmasOrder()
        {
            if (!inBrackets)
            {
                Debug.Log("Bidmas order called");
                //Calculation(numberList[i - 1], numberList[i + 1], numberList[i], i - 1, i + 1);
                if (FoundOperator(1006) && !inBrackets)
                {
                    //First bracket found
                    //BracketsArePresent();
                    inBrackets = true;
                    BracketOverride.instance.NewBidmasOrder();

                }
                else if (FoundOperator(1005))
                {
                    //Indicies (Square the number)
                    Calculation(0, 2, 0, ReturnIndex(index).Item1, index);
                }
                else if (FoundOperator(1004))
                {
                    //Division 
                    Calculation(0, 0, 0, ReturnIndex(index).Item1, ReturnIndex(index).Item2);
                }
                else if (FoundOperator(1003))
                {
                    //Multiply
                    //Find the next valid numbers 
                    Debug.Log("A index value " + ReturnIndex(index).Item1.ToString());
                    Debug.Log("B index value " + ReturnIndex(index).Item2.ToString());
                    Calculation(0, 0, 0, ReturnIndex(index).Item1, ReturnIndex(index).Item2);
                    Debug.Log("Multiply");
                }
                else if (FoundOperator(1001))
                {
                    //Add
                    //Find the next valid numbers 
                    //Calculation(0, 0, 0, ReturnIndex(index).Item1, ReturnIndex(index).Item2);
                    Calculation(0, 0, 0, ReturnIndex(index).Item1, ReturnIndex(index).Item2);
                    Debug.Log("Add");
                }
                else if (FoundOperator(1002))
                {
                    //Subtraction
                    //Find the next valid numbers 
                    Calculation(0, 0, 0, ReturnIndex(index).Item1, ReturnIndex(index).Item2);
                }
            }
        }



        private bool FoundOperator(int target)
        {
            for(int i = 0; i < numberList.Count; i++)
            {
                if(numberList[i] == target)
                {
                    index = i;
                    Debug.Log("index " + index.ToString());
                    return true;
                }
            }
            return false;
        }

        public Tuple<int, int> ReturnIndex(int startPoint)
        {
            int a = 0;
            int b = 0;
            bool aAssigned = false;
            bool bAssigned = false;
            for(int i = startPoint; i < numberList.Count; i++)
            {
                if(numberList[i] != 0 && numberList[i] < 1000 && !bAssigned)
                {
                    //Get the index of the number after the operation
                    b = i;
                    bAssigned = true;
                }
            }

            for(int i = startPoint; i >= 0; i--)
            {
                //Debug.Log("A index value from tuble " + i.ToString());
                if(numberList[i] != 0 && numberList[i] < 1000 && !aAssigned)
                {
                    //Get the index of the number before the operation
                    a = i;
                    aAssigned = true;
                }
            }
            
            //Return the start and end points respectively 
            return new Tuple<int, int>(a, b);

        }

        

        //User's actions call this function
        public void SubmitEquation()
        {
            int count = 0;
            //Calculation(5, 5, 101);
            //Uncommented when finalised
            //BidmasOrder();


            if (GameState.instance.CurrentState == GameState.State.Game)
            {
                for (int i = 0; i < numberList.Count; i++)
                {

                    for (int z = 0; z < numberList.Count; z++)
                    {
                        if (numberList[z] == 0)
                        {
                            //Used to find the point where only one numbered value exisits in the list
                            count++;
                        }
                    }


                    if (count == numberList.Count - 1)
                    {
                        RemovePlayerTotal();
                        Debug.Log("Final answer found: " + playerTotal.ToString());
                        if (playerTotal == TargetValue.instance.Result)
                        {

                            Debug.Log(playerTotal.ToString() + " is the correct answser!");

                            //Increase score 
                            ScoreValue.instance.UpdateScore(100);

                            //Have some form of feedback to the player 
                            TeacherScript.instance.ChangeAnimation("Correct");
                            scoreText.text = "Correct, well done! Your current score is: " + ScoreValue.instance.Score.ToString();

                            //Clear the blocks currently on the placers
                            ClearBlocks();

                            //Generate a new number
                            TargetValue.instance.UpdateResult();

                            break;
                        }
                        else
                        {
                            TeacherScript.instance.ChangeAnimation("Incorrect");
                            scoreText.text = "Not quite, have another try";
                        }

                    }
                    else
                    {
                        try
                        {
                            BidmasOrder();
                        }
                        catch (Exception exception)
                        {
                            //Throw an exception if an invalid equation is given
                            Debug.Log(exception);
                        }
                    }
                    count = 0;
                }
            }
            else
            {
                ClearBlocks();
                //Call UIManager with the value of the block
                UIManager.instance.CheckValue(numberList[0]);
                
            }
        }

        private void RemovePlayerTotal()
        {
            for(int i = 0; i < numberList.Count; i++)
            {
                //Used to remove the players total from the list after the final answer has been found
                if(numberList[i] != 0)
                {
                    numberList[i] = 0;
                }
            }
        }

        private void ClearBlocks()
        {
            GameObject[] placers = GameObject.FindGameObjectsWithTag("TriggerArea");
            foreach(GameObject placer in placers)
            {
                //Clear all the blocks on the placers to reset it
                placer.GetComponent<BlockTriggerArea>().RemoveBlocksOnPlacer();
            }
        }

        public void RemoveValues(int index)
        {
            //Used from object pooler to set reset the index of each placer when the total amount of placers has been changed
            numberList[index] = 0;
        }
    }
}

