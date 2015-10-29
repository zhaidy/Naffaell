using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MainPage : System.Web.UI.Page
{
    //int nRows = 16;
    //int nCols = 16;
    //int noOfMines = 30;
    //int totalBtn = 0;
    //int mineAround = 0;
    //List<int> mineButtons = new List<int>();
    //int nOpenMineCount = 0;
    //int nCurrentMineCount = 0;
    //List<mineinfo> mineinfolist;

    //public struct mineinfo
    //{
    //    public String mineid;
    //    public int row;
    //    public int col;
    //    public int minecount;

    //};
    protected void Page_Load(object sender, EventArgs e)
    {
        //totalBtn = nRows * nCols;
        //GenerateMines();

        //int id = 0;
        //for (int i = 0; i < nRows; i++)
        //{
        //    TableRow row = new TableRow();
        //    row.ID = i.ToString();
        //    for (int a = 0; a < nCols; a++)
        //    {
        //        TableCell cell = new TableCell();
        //        cell.ID = a.ToString();
        //        Button btn = new Button();
        //        Label lbl = new Label();
        //        lbl.Style.Add("z-index", "999");
        //        lbl.Style.Add("position", "absolute");
        //        btn.ID = id.ToString();
        //        btn.Style.Add("z-index", "-1");
        //        btn.Attributes.Add("OnClick", "return false");
        //        btn.Attributes.Add("class", "button");
        //        btn.UseSubmitBehavior = false;
        //        btn.CausesValidation = false;
        //        if (!mineButtons.Contains(id))
        //        {
        //            if (CalculateNumberForButton(i, a) != 0)
        //            {
        //                lbl.Text = CalculateNumberForButton(i, a).ToString();
        //                btn.ID = id.ToString() + "+" + CalculateNumberForButton(i, a).ToString();
        //            }
        //        }
        //        else
        //        {
        //            lbl.Text = "-1";
        //            btn.ID = id.ToString() + "-1";
        //        }
        //        id++;
        //        cell.Controls.Add(lbl);
        //        cell.Controls.Add(btn);
        //        row.Controls.Add(cell);
        //    }
        //    Table1.Controls.Add(row);
        //}
    }

    //[WebMethod]
    //public static void btnClick(int btnId)
    //{

    //}
    //public void GenerateMines()
    //{
    //    Random rnd = new Random(DateTime.Now.Second);
    //    int number = rnd.Next(1, totalBtn) % totalBtn;
    //    //int number = rnd.Next(1, 27) % 27;
    //    mineButtons.Add(number);

    //    for (int count = 0; count < (nRows - 1);)
    //    {
    //        number = rnd.Next(1, totalBtn) % totalBtn;
    //        //number = rnd.Next(1, 27) % 27;
    //        if (true == mineButtons.Contains(number)) continue;
    //        else
    //        {
    //            mineButtons.Add(number);
    //            count++;
    //        }
    //    }

    //}
    //protected void Button1_Click(object sender, EventArgs e)
    //{

    //}
    //public int CalculateNumberForButton(int row, int col)
    //{
    //    int digit = row * nRows + col;

    //    int minecount = 0;

    //    if (0 == row && 0 == col)
    //    {
    //        digit = col + 1;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        digit = col + nCols;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        digit = col + nCols + 1;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        if (col > 0)
    //        {
    //            digit = col - 1;

    //            if (mineButtons.Contains(digit))
    //            {
    //                minecount += 1;
    //            }

    //            digit = col + nCols - 1;

    //            if (mineButtons.Contains(digit))
    //            {
    //                minecount += 1;
    //            }
    //        }
    //        return minecount;
    //    }

    //    if ((0 == row) && (col > 0))
    //    {
    //        digit = col - 1;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        if (col != (nCols - 1))
    //        {
    //            digit = col + 1;

    //            if (mineButtons.Contains(digit))
    //            {
    //                minecount += 1;
    //            }
    //        }

    //        digit = nRows + col - 1;
    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        digit = nRows + col;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        if (col != (nCols - 1))
    //        {
    //            digit = nRows + col + 1;

    //            if (mineButtons.Contains(digit))
    //            {
    //                minecount += 1;
    //            }
    //        }

    //        return minecount;
    //    }

    //    if ((0 == col) && (row > 0))
    //    {
    //        digit = (row - 1) * nRows;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        digit = (row - 1) * nRows + 1;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        digit = row * nRows + 1;
    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        digit = (row + 1) * nRows;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        digit = (row + 1) * nRows + 1;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        return minecount;


    //    }
    //    if (row > 0 && col > 0)
    //    {

    //        int tRow = row - 1;
    //        digit = tRow * nRows + col - 1;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        digit = tRow * nRows + col;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        if (col != (nCols - 1))
    //        {
    //            digit = tRow * nRows + col + 1;

    //            if (mineButtons.Contains(digit))
    //            {
    //                minecount += 1;
    //            }
    //        }

    //        //currentrow
    //        digit = row * nRows + col - 1;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        if (col != (nCols - 1))
    //        {
    //            digit = row * nRows + col + 1;

    //            if (mineButtons.Contains(digit))
    //            {
    //                minecount += 1;
    //            }
    //        }

    //        //next row
    //        tRow = row + 1;
    //        digit = tRow * nRows + col - 1;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        digit = tRow * nRows + col;

    //        if (mineButtons.Contains(digit))
    //        {
    //            minecount += 1;
    //        }

    //        if (col != (nCols - 1))
    //        {
    //            digit = tRow * nRows + col + 1;

    //            if (mineButtons.Contains(digit))
    //            {
    //                minecount += 1;
    //            }
    //        }


    //        return minecount;
    //    }

    //    return -1;
    //}
}