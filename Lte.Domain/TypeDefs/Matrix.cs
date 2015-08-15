using System;

namespace Lte.Domain.TypeDefs
{
    public class Matrix
    {
        private double[,] m_data;

        public Matrix(int row)
        {
            m_data = new double[row, row];
        }

        public Matrix(int row, int col)
        {
            m_data = new double[row, col];
        }

        public Matrix(Matrix m)
        {
            int row = m.Row;
            int col = m.Col;
            m_data = new double[row, col];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                    m_data[i, j] = m[i, j];
        }

        public Matrix(double[,] values)
        {
            int row = values.GetLength(0);
            int col = values.GetLength(1);
            m_data = new double[row, col];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                    m_data[i, j] = values[i, j];
        }

        public Matrix(int[,] values)
        {
            int row = values.GetLength(0);
            int col = values.GetLength(1);
            m_data = new double[row, col];
            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                    m_data[i, j] = values[i, j];
        }

        public int[,] IntValues {
            get
            {
                int[,] result = new int[Row, Col];
                for(int i=0;i<Row;i++)
                    for (int j = 0; j < Col; j++)
                        result[i, j] = (int)m_data[i, j];
                return result;
            }
        }

        public void SetUnit()
        {
            for (int i = 0; i < m_data.GetLength(0); i++)
                for (int j = 0; j < m_data.GetLength(1); j++)
                    m_data[i, j] = ((i == j) ? 1 : 0);
        }

        public void SetValue(double d)
        {
            for (int i = 0; i < m_data.GetLength(0); i++)
                for (int j = 0; j < m_data.GetLength(1); j++) 
                    m_data[i, j] = d;
        }

        public int Row
        {
            get { return m_data.GetLength(0); }
        }

        public int Col
        {
            get { return m_data.GetLength(1); }
        }

        public double this[int row, int col]
        {
            get { return m_data[row, col]; }
            set { m_data[row, col] = value; }
        }

        public Matrix Exchange(int i, int j)
        {
            for (int k = 0; k < Col; k++)
            {
                double temp = m_data[i, k];
                m_data[i, k] = m_data[j, k];
                m_data[j, k] = temp;
            }
            return this;
        }

        public Matrix Multiple(int index, double mul)
        {
            for (int j = 0; j < Col; j++)
            {
                m_data[index, j] *= mul;
            }
            return this;
        }

        public Matrix MultipleAdd(int index, int src, double mul)
        {
            for (int j = 0; j < Col; j++)
            {
                m_data[index, j] += m_data[src, j]*mul;
            }
            return this;
        }

        public Matrix Transpose()
        {
            Matrix ret = new Matrix(Col, Row);
            for (int i = 0; i < Row; i++)
                for (int j = 0; j < Col; j++)
                {
                    ret[j, i] = m_data[i, j];
                }
            return ret;
        }

        public static Matrix operator +(Matrix lhs, Matrix rhs)
        {
            if (lhs.Row != rhs.Row) //异常             
            {
                Exception e = new Exception("相加的两个矩阵的行数不等");
                throw e;
            }
            if (lhs.Col != rhs.Col) //异常             
            {
                Exception e = new Exception("相加的两个矩阵的列数不等");
                throw e;
            }
            int row = lhs.Row;
            int col = lhs.Col;
            Matrix ret = new Matrix(row, col);

            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                {
                    double d = lhs[i, j] + rhs[i, j];
                    ret[i, j] = d;
                }
            return ret;
        }

        public static Matrix operator -(Matrix lhs, Matrix rhs)
        {
            if (lhs.Row != rhs.Row) //异常             
            {
                Exception e = new Exception("相减的两个矩阵的行数不等");
                throw e;
            }
            if (lhs.Col != rhs.Col) //异常             
            {
                Exception e = new Exception("相减的两个矩阵的列数不等");
                throw e;
            }
            int row = lhs.Row;
            int col = lhs.Col;
            Matrix ret = new Matrix(row, col);
            for (int i = 0; i < row; i++)
                for (int j = 0; j < col; j++)
                {
                    double d = lhs[i, j] - rhs[i, j];
                    ret[i, j] = d;
                }
            return ret;
        }

        public static Matrix operator *(Matrix lhs, Matrix rhs)
        {
            if (lhs.Col != rhs.Row) //异常             
            {
                Exception e = new Exception("相乘的两个矩阵的行列数不匹配");
                throw e;
            }
            Matrix ret = new Matrix(lhs.Row, rhs.Col);
            for (int i = 0; i < lhs.Row; i++)
            {
                for (int j = 0; j < rhs.Col; j++)
                {
                    double temp = 0;
                    for (int k = 0; k < lhs.Col; k++)
                    {
                        temp += lhs[i, k]*rhs[k, j];
                    }
                    ret[i, j] = temp;
                }
            }
            return ret;
        }

        public static Matrix operator /(Matrix lhs, Matrix rhs)
        {
            return lhs * rhs.Inverse();
        }

        public static Matrix operator +(Matrix m)
        {
            Matrix ret = new Matrix(m);             
            return ret;
        }

        public static Matrix operator -(Matrix m)
        {
            Matrix ret = new Matrix(m);
            for (int i = 0; i < ret.Row; i++)
                for (int j = 0; j < ret.Col; j++)
                {
                    ret[i, j] = -ret[i, j];
                }
            return ret;
        }

        public static Matrix operator *(double d, Matrix m)
        {
            Matrix ret = new Matrix(m);
            for (int i = 0; i < ret.Row; i++)
                for (int j = 0; j < ret.Col; j++) ret[i, j] *= d;
            return ret;
        }

        public static Matrix operator /(double d, Matrix m)
        {
            return d * m.Inverse();
        }

        int Pivot(int row)
        {
            int index = row;
            for (int i = row + 1; i < Row; i++)
            {
                if (m_data[i, row] > m_data[index, row]) index = i;
            }
            return index;
        }

        public Matrix Inverse()
        {
            if (Row != Col) //异常,非方阵             
            {
                Exception e = new Exception("求逆的矩阵不是方阵");
                throw e;
            }

            Matrix tmp = new Matrix(this);
            Matrix ret = new Matrix(Row);
            ret.SetUnit();

            double dMul;
            for (int i = 0; i < Row; i++)
            {
                int maxIndex = tmp.Pivot(i);
                if ((int)tmp.m_data[maxIndex, i] == 0)
                {
                    Exception e = new Exception("求逆的矩阵的行列式的值等于0,");
                    throw e;
                }
                if (maxIndex != i)
                {
                    tmp.Exchange(i, maxIndex);
                    ret.Exchange(i, maxIndex);
                }
                ret.Multiple(i, 1/tmp[i, i]);
                tmp.Multiple(i, 1/tmp[i, i]);
                for (int j = i + 1; j < Row; j++)
                {
                    dMul = -tmp[j, i]/tmp[i, i];
                    tmp.MultipleAdd(j, i, dMul);
                    ret.MultipleAdd(j, i, dMul);
                }
            }

            for (int i = Row - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    dMul = -tmp[j, i]/tmp[i, i];
                    tmp.MultipleAdd(j, i, dMul);
                    ret.MultipleAdd(j, i, dMul);
                }
            }
            return ret;
        }

        public bool IsSquare()
        {
            return Row == Col;
        }

        public bool IsSymmetric()
        {
            if (Row != Col) return false;
            for (int i = 0; i < Row; i++)
                for (int j = i + 1; j < Col; j++)
                    if ((int)m_data[i, j] != (int)m_data[j, i]) 
                        return false;
            return true;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                    s += string.Format("{0} ", m_data[i, j]);
                s += "\r\n";
            }
            return s;
        }
    }
}
