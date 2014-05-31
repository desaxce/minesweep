using System.Windows.Forms;
using System.Drawing;
using System;

public class Simple : Form
{
	private int WIDTH;
	private int HEIGHT;
	private int RIGHT_SPACE = 10;
	private int LEFT_SPACE = 10;
	private int TOP_SPACE = 20;
	private int BOTTOM_SPACE = 40;
	private int PANEL_SPACE = 8;
	private int BUTTONS_WIDTH = 25;
	private int BUTTONS_HEIGHT = 30;
	private int[][] matrix;
	private Button[] btn;
	private bool[] visited;
	private int COL;
	private int ROW;
	private int NB_MINES = 25;
	private int compteur = 0;

	public Simple(int col, int row)
	{
		int i, j;
		ROW = row;
		COL = col;
		WIDTH = COL * (BUTTONS_WIDTH) + RIGHT_SPACE + LEFT_SPACE;
		HEIGHT = ROW * (BUTTONS_HEIGHT) + TOP_SPACE + BOTTOM_SPACE;
		matrix = new int[row+2][];
		for (i = 0; i < row + 2; ++i) {
			matrix[i] = new int[col+2];
			for (j = 0; j < col + 2; ++j) {
				matrix[i][j] = 0;
			}
		}

		Random rnd = new Random();
		int cnt = 0;
		while (cnt < NB_MINES) {
			int rx = rnd.Next(1, COL+1);
			int ry = rnd.Next(1, ROW+1);
			Console.WriteLine("rx = " + rx);
			Console.WriteLine("ry = " + ry);
			if (matrix[ry][rx]==0) {
				matrix[ry][rx] = 1;
				++cnt;
			}
		}

		Text = "Simple";
		Size = new Size(WIDTH, HEIGHT);
		btn = new Button[row * col];
		visited = new bool[row * col];

		for (i = 0; i < row * col; ++i) {
			int ii = i; // you cannot simply put "i" in Onclick
			visited[i] = false;
			btn[i] = new Button();
			btn[i].Width = BUTTONS_WIDTH;
			btn[i].Height = BUTTONS_HEIGHT;
			btn[i].TextAlign = ContentAlignment.MiddleCenter;
			btn[i].Font = new Font("Georgia", 16);
			btn[i].Click += (sender, e) => OnClick(sender, e, ii);
			Controls.Add(btn[i]);
		}

		int PANEL_HEIGHT = BUTTONS_HEIGHT + PANEL_SPACE;
		Panel panel = new Panel();
		panel.Height = PANEL_HEIGHT;
		panel.Dock = DockStyle.Fill;
		panel.Parent = this;


		for (i = 0; i < row * col; ++i) {
			int x = BUTTONS_WIDTH * ((i%col)+1) + RIGHT_SPACE;
			int y = PANEL_HEIGHT + BUTTONS_HEIGHT * (i/col - 1);
			btn[i].Text = " ";
			btn[i].Parent = panel;
			btn[i].Location = new Point(WIDTH-x, y);
			btn[i].Anchor = AnchorStyles.Right;
		}

		MainMenu mainMenu = new MainMenu();
		MenuItem file = mainMenu.MenuItems.Add("&File");
		file.MenuItems.Add(new MenuItem("E&xit", 
			new EventHandler(this.OnExit), Shortcut.CtrlX));
		Menu = mainMenu;
		file.MenuItems.Add(new MenuItem("N&ew Game", 
			new EventHandler(this.OnNewGame)));

		// ToolTip btnTlp = new ToolTip();

		// btnTlp.SetToolTip(this, "This is a Form");


		// Controls.Add(button);

		CenterToScreen();
	}

	void OnClick(object sender, EventArgs e, int i) {
		visited[i] = true;
		int r = i % COL + 1;
		int q = i / COL + 1;
		if (matrix[q][r]==0) {

			++compteur;
			int result = matrix[q-1][r] + matrix[q-1][r-1] + matrix[q-1][r+1]
				+ matrix[q+1][r] + matrix[q+1][r-1] + matrix[q+1][r+1]
				+ matrix[q][r-1] + matrix[q][r+1];
			
			if (result == 0) {
				btn[i].TextAlign = ContentAlignment.MiddleCenter;
				btn[i].Text = ".";
				if (r != COL && !(visited[i+1])) {
					OnClick(sender, e, i+1);
				}
				if (r != 1 && !(visited[i-1])) {
					OnClick(sender, e, i-1);
				}
				if (q != 1 && i >= COL && !(visited[i-COL])) {
					OnClick(sender, e, i-COL);
				}
				if (q != 1 && r != 1 && i >= COL+1 && !(visited[i-COL-1])) {
					OnClick(sender, e, i-COL-1);
				}
				if (q != 1 && r != COL && i >= COL-1 && !(visited[i-COL+1])) {
					OnClick(sender, e, i-COL+1);
				}
				if (q != ROW && i < (ROW-1)*COL && !(visited[i+COL])) {
					OnClick(sender, e, i+COL);
				}
				if (q != ROW && r != COL && i < ROW*COL-COL-1 && !(visited[i+COL+1])) {
					OnClick(sender, e, i+COL+1);
				}
				if (q != ROW && r != 1 && i < ROW*COL-COL+1 && !(visited[i+COL-1])) {
					OnClick(sender, e, i+COL-1);
				}
			}
			else {
				if (result ==1) {
					btn[i].ForeColor = Color.Blue;
				}
				if (result == 2) {
					btn[i].ForeColor = Color.Orange;
				}
				if (result == 3) {
					btn[i].ForeColor = Color.Red;
				}
				if (result == 4) {
					btn[i].ForeColor = Color.Purple;
				}
				btn[i].TextAlign = ContentAlignment.MiddleCenter;
				btn[i].Text = result.ToString();

			}
			if (compteur == ROW * COL - NB_MINES + 1) {
				Console.WriteLine("You won\n");
			}
		}
		else {
			btn[i].BackColor = Color.Red;
			int j, k;
			for (j = 0; j < ROW; ++j) {
				for (k = 0; k < COL; ++k) {
					if (matrix[j+1][k+1]==1) {
						btn[j*ROW+k].Text = "*";
						btn[j*ROW+k].TextAlign = ContentAlignment.MiddleCenter;
					}
				}
			}
			Console.WriteLine("You lost\n");
		}
	}

	void OnEnter(object sender, EventArgs e) {
		Console.WriteLine("Button Entered");
	}

	void OnExit(object sender, EventArgs e) {
		Close();
	}
	void OnNewGame(object sender, EventArgs e) {
		new Simple(COL, ROW);
	}

}

class MApplication {

	public static void Main() {
		int col = 15;
		int row = 15;

		Simple easy = new Simple(col, row);
		Application.Run(easy);
	}
}


