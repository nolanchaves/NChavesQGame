using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NChavesQGame
{
	internal interface ILevelDesigner
	{
		void SaveLevel(string path);
		void CreateGrid(int rows, int columns);

	}
}
