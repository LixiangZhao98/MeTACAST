using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LixaingZhao.MeTACAST{
public class Enum
{
[Serializable]
public enum SelectionTech { Point, Brush, Paint, BaseLine };
[Serializable]
public enum Dataset { disk, uniform_Lines, ball_hemisphere, ununiform_Lines, Flocculentcube1, strings, Flocculentcube2, Flocculentcube3, galaxy, nbody1, nbody2, training_torus,training_sphere,training_pyramid,training_cylinder, random_sphere, three_rings, multiEllipsolds, fiveellipsolds, stringf, stringf1, snap_C02_200_127_animation };
[Serializable]
public enum GRIDNum { grid64, grid100, grid200 };
}
}