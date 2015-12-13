using System;
using System.Collections;

public class Matrix {

	public int n;
	public int[,] mat;
	private System.Random rng; // [seed=1010, n=3]
	// bounds on entries matrix
	private int lowerbound = -5;
	private int upperbound = 6; // upperbound is exclusive

	public Matrix(int nn) {
		n = nn;
		mat = new int[n, n];
		rng = new System.Random (1020);
		InitMatrix ();
	}

	// TODO improve
	private int CalcDeterminant() {
		if (n != 3)
			throw new NotImplementedException();
		int res = 0;
		res+=mat[0,0]*(mat[1,1]*mat[2,2]-mat[2,1]*mat[1,2]);
		res-=mat[1,0]*(mat[0,1]*mat[2,2]-mat[2,1]*mat[0,2]);
		res+=mat[2,0]*(mat[0,1]*mat[1,2]-mat[1,1]*mat[0,2]);
		return res;
	}

	public void InitMatrix() {
		do {
			for (int i=0; i<n; i++) {
				for (int j=0; j<n; j++) {
					mat [i, j] = rng.Next(lowerbound,upperbound);
				}
			}
		}while(CalcDeterminant()==0);
	}


}
