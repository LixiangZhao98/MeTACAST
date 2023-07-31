using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatinSquare 
{
  
    
    [SerializeField]
   static public List<Task> tasks = new List<Task>();
    static SelectionTech A = SelectionTech.Point;
    static SelectionTech B = SelectionTech.Brush;
    static SelectionTech C = SelectionTech.Paint;
    static SelectionTech D = SelectionTech.BaseLine;


    static Dataset d1=Dataset.disk;
    static Dataset d2= Dataset.uniform_Lines;//
    static Dataset d3=Dataset.ball_hemisphere;
    static Dataset d4=Dataset.ununiform_Lines;
    static Dataset d5 = Dataset.Flocculentcube1;

    static Dataset Apre=Dataset.nbody2;
    static Dataset Bpre = Dataset.Flocculentcube2;
    static Dataset Cpre = Dataset.fiveellipsolds;
    static Dataset Dpre = Dataset.training_torus;
    static public List<Task> GetTask(int PID) //0 to n
    {
        if (PID < 0 || PID > 31)
        {
            Debug.LogError("Wrong PID");
            return new List<Task>();
        }
        if (PID > 15)
            PID -= 16;
        int datasetOrder=(PID/4)%4;
        int methodOrder = PID % 4;
        
        if (methodOrder == 0&& datasetOrder == 0)
        {
            tasks = new List<Task>() { new Task(A, Apre,true,true), new Task(A, d1), new Task(A, d2), new Task(A, d3), new Task(A, d4), new Task(A, d5),    new Task(B, Bpre,true),   new Task(B, d1), new Task(B, d2), new Task(B, d3), new Task(B, d4),new Task(B, d5),   new Task(C, Cpre, true), new Task(C, d1), new Task(C, d2), new Task(C, d3), new Task(C, d4),new Task(C, d5),   new Task(D, Dpre, true), new Task(D, d1), new Task(D, d2), new Task(D, d3), new Task(D, d4), new Task(D, d5), } ;
        }
        if(methodOrder == 1 && datasetOrder == 0)
        {
            tasks = new List<Task>() { new Task(B, Bpre, true, true), new Task(B, d1), new Task(B, d2), new Task(B, d3), new Task(B, d4), new Task(B, d5),   new Task(C, Cpre, true),  new Task(C, d1), new Task(C, d2), new Task(C, d3), new Task(C, d4),new Task(C, d5),   new Task(D, Dpre, true),  new Task(D, d1), new Task(D, d2), new Task(D, d3), new Task(D, d4),new Task(D, d5),  new Task(A, Apre, true), new Task(A, d1), new Task(A, d2), new Task(A, d3), new Task(A, d4), new Task(A, d5), };
        }
        if(methodOrder == 2 && datasetOrder == 0)
        {
            tasks = new List<Task>() { new Task(C, Cpre, true, true), new Task(C, d1), new Task(C, d2), new Task(C, d3), new Task(C, d4), new Task(C, d5),   new Task(D, Dpre, true),  new Task(D, d1), new Task(D, d2), new Task(D, d3), new Task(D, d4),new Task(D, d5),   new Task(A, Apre, true), new Task(A, d1), new Task(A, d2), new Task(A, d3), new Task(A, d4), new Task(A, d5),  new Task(B, Bpre, true), new Task(B, d1), new Task(B, d2), new Task(B, d3), new Task(B, d4), new Task(B, d5), };

        }
        if (methodOrder == 3 && datasetOrder == 0)
        {
            tasks = new List<Task>() { new Task(D, Dpre, true, true), new Task(D, d1), new Task(D, d2), new Task(D, d3), new Task(D, d4), new Task(D, d5),   new Task(A, Apre, true), new Task(A, d1), new Task(A, d2), new Task(A, d3), new Task(A, d4),new Task(A, d5),    new Task(B, Bpre, true), new Task(B, d1), new Task(B, d2), new Task(B, d3), new Task(B, d4), new Task(B, d5),  new Task(C, Cpre, true), new Task(C, d1), new Task(C, d2), new Task(C, d3), new Task(C, d4), new Task(C, d5), };

        }


        if (methodOrder == 0 && datasetOrder == 1)
        {
            tasks = new List<Task>() { new Task(A, Apre, true, true), new Task(A, d2), new Task(A, d3), new Task(A, d4), new Task(A, d1), new Task(A, d5),    new Task(B, Bpre, true), new Task(B, d2), new Task(B, d3), new Task(B, d4), new Task(B, d1), new Task(B, d5),  new Task(C, Cpre, true), new Task(C, d2), new Task(C, d3), new Task(C, d4), new Task(C, d1), new Task(C, d5),  new Task(D, Dpre, true), new Task(D, d2), new Task(D, d3), new Task(D, d4) ,new Task(D, d1), new Task(D, d5), };
        }
        if (methodOrder == 1 && datasetOrder == 1)
        {
            tasks = new List<Task>() { new Task(B, Bpre, true, true), new Task(B, d2), new Task(B, d3), new Task(B, d4), new Task(B, d1), new Task(B, d5),    new Task(C, Cpre, true), new Task(C, d2), new Task(C, d3), new Task(C, d4), new Task(C, d1),new Task(C, d5),   new Task(D, Dpre, true), new Task(D, d2), new Task(D, d3), new Task(D, d4), new Task(D, d1),new Task(D, d5),   new Task(A, Apre, true), new Task(A, d2), new Task(A, d3), new Task(A, d4), new Task(A, d1), new Task(A, d5), };
        }
        if (methodOrder == 2 && datasetOrder == 1)
        {
            tasks = new List<Task>() { new Task(C, Cpre, true, true), new Task(C, d2), new Task(C, d3), new Task(C, d4), new Task(C, d1), new Task(C, d5),    new Task(D, Dpre, true), new Task(D, d2), new Task(D, d3), new Task(D, d4), new Task(D, d1),new Task(D, d5),   new Task(A, Apre, true), new Task(A, d2), new Task(A, d3), new Task(A, d4), new Task(A, d1),new Task(A, d5),   new Task(B, Bpre, true), new Task(B, d2), new Task(B, d3), new Task(B, d4), new Task(B, d1), new Task(B, d5), };
        }
        if (methodOrder == 3 && datasetOrder == 1)
        {
            tasks = new List<Task>() { new Task(D, Dpre, true, true), new Task(D, d2), new Task(D, d3), new Task(D, d4), new Task(D, d1), new Task(D, d5),    new Task(A, Apre, true), new Task(A, d2), new Task(A, d3), new Task(A, d4), new Task(A, d1), new Task(A, d5),  new Task(B, Bpre, true), new Task(B, d2), new Task(B, d3), new Task(B, d4), new Task(B, d1), new Task(B, d5),  new Task(C, Cpre, true), new Task(C, d2), new Task(C, d3), new Task(C, d4), new Task(C, d1), new Task(C, d5), };
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                        



        if (methodOrder == 0 && datasetOrder == 2)
        {
            tasks = new List<Task>() { new Task(A, Apre, true, true), new Task(A, d3), new Task(A, d4), new Task(A, d1), new Task(A, d2), new Task(A, d5),    new Task(B, Bpre, true), new Task(B, d3), new Task(B, d4), new Task(B, d1), new Task(B, d2),new Task(B, d5),   new Task(C, Cpre, true), new Task(C, d3), new Task(C, d4), new Task(C, d1), new Task(C, d2),new Task(C, d5),   new Task(D, Dpre, true), new Task(D, d3), new Task(D, d4), new Task(D, d1),new Task(D, d2) , new Task(D, d5), };
        }
        if (methodOrder == 1 && datasetOrder == 2)
        {
            tasks = new List<Task>() { new Task(B, Bpre, true, true), new Task(B, d3), new Task(B, d4), new Task(B, d1), new Task(B, d2), new Task(B, d5),    new Task(C, Cpre, true), new Task(C, d3), new Task(C, d4), new Task(C, d1), new Task(C, d2),new Task(C, d5),   new Task(D, Dpre, true), new Task(D, d3), new Task(D, d4), new Task(D, d1), new Task(D, d2),new Task(D, d5),    new Task(A, Apre, true), new Task(A, d3), new Task(A, d4), new Task(A, d1), new Task(A, d2), new Task(A, d5),};
        }
        if (methodOrder == 2 && datasetOrder == 2)
        {
            tasks = new List<Task>() { new Task(C, Cpre, true, true), new Task(C, d3), new Task(C, d4), new Task(C, d1), new Task(C, d2), new Task(C, d5),    new Task(D, Dpre, true), new Task(D, d3), new Task(D, d4), new Task(D, d1), new Task(D, d2),new Task(D, d5),   new Task(A, Apre, true), new Task(A, d3), new Task(A, d4), new Task(A, d1), new Task(A, d2),new Task(A, d5),    new Task(B, Bpre, true), new Task(B, d3), new Task(B, d4), new Task(B, d1), new Task(B, d2), new Task(B, d5),};
        }
        if (methodOrder == 3 && datasetOrder == 2)
        {
            tasks = new List<Task>() { new Task(D, Dpre, true, true), new Task(D, d3), new Task(D, d4), new Task(D, d1), new Task(D, d2), new Task(D, d5),    new Task(A, Apre, true), new Task(A, d3), new Task(A, d4), new Task(A, d1), new Task(A, d2),new Task(A, d5),   new Task(B, Bpre, true), new Task(B, d3), new Task(B, d4), new Task(B, d1), new Task(B, d2),new Task(B, d5),   new Task(C, Cpre, true), new Task(C, d3), new Task(C, d4), new Task(C, d1), new Task(C, d2), new Task(C, d5), };
        }


        if (methodOrder == 0 && datasetOrder == 3)
        {
            tasks = new List<Task>() { new Task(A, Apre, true, true), new Task(A, d4), new Task(A, d1), new Task(A, d2), new Task(A, d3), new Task(A, d5),   new Task(B, Bpre, true), new Task(B, d4), new Task(B, d1), new Task(B, d2), new Task(B, d3), new Task(B, d5),   new Task(C, Cpre, true), new Task(C, d4), new Task(C, d1), new Task(C, d2), new Task(C, d3),new Task(C, d5),    new Task(D, Dpre, true), new Task(D, d4), new Task(D, d1), new Task(D, d2), new Task(D, d3) ,new Task(D, d5), };
        }
        if (methodOrder == 1 && datasetOrder == 3)
        {
            tasks = new List<Task>() { new Task(B, Bpre, true, true), new Task(B, d4), new Task(B, d1), new Task(B, d2), new Task(B, d3),new Task(B, d5),    new Task(C, Cpre, true), new Task(C, d4), new Task(C, d1), new Task(C, d2), new Task(C, d3),new Task(C, d5),    new Task(D, Dpre, true), new Task(D, d4), new Task(D, d1), new Task(D, d2), new Task(D, d3),new Task(D, d5),    new Task(A, Apre, true), new Task(A, d4), new Task(A, d1), new Task(A, d2), new Task(A, d3)  ,new Task(A, d5),};
        }
        if (methodOrder == 2 && datasetOrder == 3)
        {
            tasks = new List<Task>() { new Task(C, Cpre, true, true), new Task(C, d4), new Task(C, d1), new Task(C, d2), new Task(C, d3),new Task(C, d5),    new Task(D, Dpre, true), new Task(D, d4), new Task(D, d1), new Task(D, d2), new Task(D, d3),new Task(D, d5),    new Task(A, Apre, true), new Task(A, d4), new Task(A, d1), new Task(A, d2), new Task(A, d3),new Task(A, d5),    new Task(B, Bpre, true), new Task(B, d4), new Task(B, d1), new Task(B, d2), new Task(B, d3) , new Task(B, d5),};
        }
        if (methodOrder == 3 && datasetOrder == 3)
        {
            tasks = new List<Task>() { new Task(D, Dpre, true, true), new Task(D, d4), new Task(D, d1), new Task(D, d2), new Task(D, d3),new Task(D, d5),    new Task(A, Apre, true), new Task(A, d4), new Task(A, d1), new Task(A, d2), new Task(A, d3),new Task(A, d5),    new Task(B, Bpre, true), new Task(B, d4), new Task(B, d1), new Task(B, d2), new Task(B, d3),new Task(B, d5),   new Task(C, Cpre, true), new Task(C, d4), new Task(C, d1), new Task(C, d2), new Task(C, d3), new Task(C, d5), } ;
        }

        return tasks;
    }
}

[SerializeField]
public struct Task
{
    public Dataset d { get; }
    public SelectionTech s { get; }

    public bool isPre { get; }

    public bool skipQuestionair;
    public Task (SelectionTech s_,Dataset d_ ,bool isPre_=false,bool skipQuestionair_ = false)
    {
        d=d_;
       s=s_;
        isPre=isPre_;
        skipQuestionair = skipQuestionair_;
    }

    public void SetSkip(bool b)
    {
        skipQuestionair = b;
    }
    //public override string ToString() => $"({d.ToString()}, {s.ToString()})";
}

