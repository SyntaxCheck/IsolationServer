using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NeuralNetworkDataSet
{
    public int GridSize { get; set; }
    public string[] GridState { get; set; }
    public int[] PossibleMoves { get; set; }
    public int MoveMade { get; set; }

    public NeuralNetworkDataSet()
    {
    }

    public DataSet ToDataSet()
    {
        double[] mapInputs = GridStringToDoubleGrid(GridState);
        double[] inputs = new double[PossibleMoves.Length + mapInputs.Length];

        for (int i = 0; i < PossibleMoves.Length; i++)
        {
            inputs[i] = PossibleMoves[i];
        }
        for (int i = 0; i < mapInputs.Length; i++)
        {
            inputs[i + PossibleMoves.Length] = mapInputs[i];
        }

        return new DataSet(inputs, MoveMadeToArray(MoveMade));
    }
    public double[] GridStringToDoubleGrid(string[] gridIn)
    {
        double[] rtnGrid = new double[gridIn.Length];

        for (int i = 0; i < gridIn.Length; i++)
        {
            rtnGrid[i] = StringToDouble(gridIn[i]);
        }

        return rtnGrid;
    }
    public double StringToDouble(string s)
    {
        switch (s)
        {
            case " ":
                return 0;
            case "X":
                return 0.33;
            case "O":
                return 0.66;
            case "U":
                return 1;
        }

        return 0;
    }
    public double[] MoveMadeToArray(int moveMade)
    {
        double[] rtn = new double[8];

        for (int i = 0; i < 8; i++)
        {
            if (i == moveMade)
                rtn[i] = 1;
            else
                rtn[i] = 0;
        }

        return rtn;
    }
}