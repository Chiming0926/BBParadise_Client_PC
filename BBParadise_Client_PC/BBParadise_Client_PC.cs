using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BBParadise_Client_PC
{
    public partial class BBParadise_Client_PC : Form
    {

        string gguid = "7245577f-4961-7642-a64c-ba5bb008892c";
        string sguid = "52a06444-ff13-654b-bfa1-29da9f7124dd";
        byte[] certificate = {0x43, 0x13, 0xbd, 0x25, 0x4e, 0x7b, 0xa3, 0x4b, 0x86, 0x13, 0xa8, 0xa5, 0x49,
                            0xcf, 0xd1, 0x5e, 0x58, 0x34, 0x2c, 0xda, 0xb, 0x9f, 0xc, 0x41, 0x8a, 0x3e,
                            0xd4, 0x71, 0x5a, 0xb, 0x3d, 0x41, 0x57, 0x1c, 0x53, 0xed, 0xf, 0x52, 0x70,
                            0x47, 0x93, 0xb5, 0x9f, 0x27, 0xe3, 0xa0, 0x66, 0x2d, 0x86, 0x55, 0x5b, 0x9c,
                            0x4b, 0x33, 0xd8, 0x40, 0xbb, 0x66, 0xf1, 0x8c, 0x67, 0x19, 0x8a, 0x4, 0x1a,
                            0x14, 0x28, 0xa7, 0x67, 0x95, 0x72, 0x4c, 0x8c, 0xfb, 0xa, 0xf5, 0x4a, 0x12,
                            0x49, 0x53, 0x6d, 0xf7, 0xa0, 0xd4, 0x73, 0x96, 0x52, 0x43, 0x8e, 0x54, 0x79,
                            0xfa, 0x48, 0xeb, 0x5b, 0xaf, 0xd1, 0x64, 0x20, 0x3d, 0x49, 0xa5, 0xbc, 0x40,
                            0x89, 0xa2, 0xb6, 0xc5, 0x6f, 0xd6, 0xac, 0xfe, 0x2f, 0x92, 0x4d, 0xbc, 0x3f,
                            0xbd, 0x4b, 0x4d, 0x90, 0xf8, 0x50, 0xf2, 0x2, 0x16, 0x75, 0xd4};
		string sguid_game = "c4345a29-310f-d241-b95c-77928bf819c6";
		

        List<UserModel> m_user = new List<UserModel>();
        private static int MAX_USER = 20;


        public BBParadise_Client_PC()
        {
            InitializeComponent();
            login();
        }

        void createNewUser()
        {
            for (int i=1; i<11; i++)
            {
                string username = "bbhappy" + i.ToString("000");
                string password = "12345678";
                string email = "bbhappy" + i.ToString("000") + "@gmail.com"; 
                Regist(username, password, email);
             //   System.Threading.Thread.Sleep(1000);
            }
        }

        void CB_Regist(int code, object token)
        {

            if (code == 0)
            {
                /* regist sucessful */
                string[] reg = token as string[];
                string acc = reg[0];
                string pw = reg[1];
                string mail = reg[2];
                Console.WriteLine("Regist Successed - Account:" + acc + " / Password:" +
                 pw + " E-Mail" + mail);

                //ArcaletLaunch(acc, pw, mail);
            }
            else
            {
                Console.WriteLine("Regist Failed - Error:" + code);
            }
            //error_code = code;
        }

        void Regist(string username, string password, string mail)
        {
            string[] registToken = new string[] { username, password, mail };
            ArcaletSystem.ApplyNewUser(gguid, certificate, username, password,
             mail, CB_Regist, registToken);
        }


        void CB_Login(int code, ArcaletGame game)
        {
            if (code == 0)
            {

            }
            else
            {

            }
        }



        void CB_ArcaletLaunch(int code, ArcaletGame game)
        {
            if (code == 0)
            {
                for (int i = 0; i < m_user.Count; i++)
                {
                    if (m_user[i].account == game.gameUserid)
                    {
                        Console.WriteLine("OK");
                        m_user[i].userStatus = new Label();
                        m_user[i].userStatus.Text = "Login Successfully";

                        m_user[i].userStatus.Location = new Point(150, 20 + (i+1) * 30);
                        this.Controls.Add(m_user[i].userStatus);

                        m_user[i].matchButton = new Button();
                        m_user[i].matchButton.Text = "Match";
                        m_user[i].matchButton.Name = game.gameUserid;
                        m_user[i].matchButton.Click += new EventHandler(btMatch_Click);
                        m_user[i].matchButton.Location = new Point(300, 15 + (i + 1) * 30);
                        this.Controls.Add(m_user[i].matchButton);

                        //StartCoroutine("DPLinkTimer");
                        m_user[i].ag.SendOnClose("quit:" + m_user[i].ag.gameUserid + "/" + m_user[i].ag.poid);
                        m_user[i].ag.Send("new:" + m_user[i].ag.gameUserid + "/" + m_user[i].ag.poid);

                        m_user[i].matchinfo = new MatchInfo();
                        m_user[i].matchinfo.GenerateMatchCode();

						m_user[i].upButton = new Button();
                        m_user[i].upButton.Text = "Up";
                        m_user[i].upButton.Name = game.gameUserid;
                        m_user[i].upButton.Click += new EventHandler(btUp_Click);
                        m_user[i].upButton.Location = new Point(450, 15 + (i + 1) * 30);
                        this.Controls.Add(m_user[i].upButton);
                    }
                }
            }
            else
            {
                Console.WriteLine("GG code = " + code);
            }
        }

        void btMatch_Click(object sender, EventArgs e)
        {
            Button tmp = (Button)sender;
            for (int i = 0; i < m_user.Count; i++)
            {
                if (m_user[i].account == tmp.Name)
                {
                    string msg = m_user[i].ag.gameUserid + "/" + m_user[i].ag.poid + "/" + m_user[i].ag.gameUserid + "/" + m_user[i].matchinfo.matchCode;
                    m_user[i].ag.PrivacySend("match:" + msg, m_user[i].dpPoid);
                }
            }
        }

		void btUp_Click(object sender, EventArgs e)
        {
            Button tmp = (Button)sender;
            for (int i = 0; i < m_user.Count; i++)
            {
                if (m_user[i].account == tmp.Name)
                {
                    string msg = "bb_move:" + m_user[i].dpPoid + "/" + "UP";
                    m_user[i].sn.Send(msg);
                }
            }
        }

        void login()
        {
            for (int i= 1; i <= MAX_USER; i++)
            {
                string username = "bbhappy" + i.ToString("000");
                string password = "12345678";
                UserModel userModel = new UserModel();
                userModel.account = username;
                Console.WriteLine(username + "/" + password);
                userModel.ag = new ArcaletGame(username, password, gguid, sguid, certificate);
                userModel.ag.onMessageIn += MainMessageIn;
                userModel.ag.onPrivateMessageIn += PrivateMessageIn;
                userModel.ag.onCompletion += CB_ArcaletLaunch;
                userModel.ag.Launch();
                m_user.Add(userModel);
                userModel.userName = new Label();
                userModel.userName.Text = username;

                userModel.userName.Location = new Point(20, 20 + i*30);
                this.Controls.Add(userModel.userName);
                
            }
        }

        void MainMessageIn(string msg, int delay, ArcaletGame game)
        {
            try
            {
                Console.WriteLine("MainMessageIn: " + msg);
                string[] cmds = msg.Split(':');
                switch (cmds[0])
                {
                    
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }

        void PrivateMessageIn(string msg, int delay, ArcaletGame game)
        {
            try
            {
                Console.WriteLine("PrivateMessageIn: " + msg);
                string[] cmds = msg.Split(':');
                switch (cmds[0])
                {
                    case "dp_new": setdpPoid(int.Parse(cmds[1])); break;
					case "dp_room": dpRoom(cmds[1]); break;
                        //    case "cancel": CancelMatchCheck(int.Parse(cmds[1])); break;
                }
            }
            catch (Exception) { }
        }

		void GameMessageIn1(string msg, int delay, ArcaletScene scene)
		{
			try 
			{
                Console.WriteLine("@ GameMsg1 >> " + msg);
			/*	string[] cmds = msg.Split(':');
	            switch (cmds[0])
	            {
					case "dp_start": game.GameStart(cmds[1]); break;
					case "dp_player": game.SetRevalInfos(cmds[1]); break;
					case "dp_slot": game.FillSlot(cmds[1]); break;
					case "dp_gameover": game.DP_GameOver(cmds[1]); break;
					case "dp_draw": game.DP_Draw(cmds[1]); break;
					case "dp_timeup": game.DP_TiemUP(cmds[1]); break;
					case "dp_sync" : game.TimerSynchronization(cmds[1], delay); break;
	            }
	            */
	        }
	        catch (Exception e) { Console.WriteLine("GameMessageIn Exception:\r\n" + e.ToString()); }
		}

		void GameMessageIn2(string msg, int delay, ArcaletScene scene)
		{
			try 
			{
                Console.WriteLine("@ GameMsg2 >> " + msg);
			/*	string[] cmds = msg.Split(':');
	            switch (cmds[0])
	            {
					case "dp_start": game.GameStart(cmds[1]); break;
					case "dp_player": game.SetRevalInfos(cmds[1]); break;
					case "dp_slot": game.FillSlot(cmds[1]); break;
					case "dp_gameover": game.DP_GameOver(cmds[1]); break;
					case "dp_draw": game.DP_Draw(cmds[1]); break;
					case "dp_timeup": game.DP_TiemUP(cmds[1]); break;
					case "dp_sync" : game.TimerSynchronization(cmds[1], delay); break;
	            }
	            */
	        }
	        catch (Exception e) { Console.WriteLine("GameMessageIn Exception:\r\n" + e.ToString()); }
		}

		void GameMessageIn3(string msg, int delay, ArcaletScene scene)
		{
			try 
			{
                Console.WriteLine("@ GameMsg3 >> " + msg);
			/*	string[] cmds = msg.Split(':');
	            switch (cmds[0])
	            {
					case "dp_start": game.GameStart(cmds[1]); break;
					case "dp_player": game.SetRevalInfos(cmds[1]); break;
					case "dp_slot": game.FillSlot(cmds[1]); break;
					case "dp_gameover": game.DP_GameOver(cmds[1]); break;
					case "dp_draw": game.DP_Draw(cmds[1]); break;
					case "dp_timeup": game.DP_TiemUP(cmds[1]); break;
					case "dp_sync" : game.TimerSynchronization(cmds[1], delay); break;
	            }
	            */
	        }
	        catch (Exception e) { Console.WriteLine("GameMessageIn Exception:\r\n" + e.ToString()); }
		}

		void GameMessageIn4(string msg, int delay, ArcaletScene scene)
		{
			try 
			{
                Console.WriteLine("@ GameMsg4 >> " + msg);
			/*	string[] cmds = msg.Split(':');
	            switch (cmds[0])
	            {
					case "dp_start": game.GameStart(cmds[1]); break;
					case "dp_player": game.SetRevalInfos(cmds[1]); break;
					case "dp_slot": game.FillSlot(cmds[1]); break;
					case "dp_gameover": game.DP_GameOver(cmds[1]); break;
					case "dp_draw": game.DP_Draw(cmds[1]); break;
					case "dp_timeup": game.DP_TiemUP(cmds[1]); break;
					case "dp_sync" : game.TimerSynchronization(cmds[1], delay); break;
	            }
	            */
	        }
	        catch (Exception e) { Console.WriteLine("GameMessageIn Exception:\r\n" + e.ToString()); }
		}

		void GameMessageIn5(string msg, int delay, ArcaletScene scene)
		{
			try 
			{
                Console.WriteLine("@ GameMsg5 >> " + msg);
			/*	string[] cmds = msg.Split(':');
	            switch (cmds[0])
	            {
					case "dp_start": game.GameStart(cmds[1]); break;
					case "dp_player": game.SetRevalInfos(cmds[1]); break;
					case "dp_slot": game.FillSlot(cmds[1]); break;
					case "dp_gameover": game.DP_GameOver(cmds[1]); break;
					case "dp_draw": game.DP_Draw(cmds[1]); break;
					case "dp_timeup": game.DP_TiemUP(cmds[1]); break;
					case "dp_sync" : game.TimerSynchronization(cmds[1], delay); break;
	            }
	            */
	        }
	        catch (Exception e) { Console.WriteLine("GameMessageIn Exception:\r\n" + e.ToString()); }
		}

		void GameMessageIn6(string msg, int delay, ArcaletScene scene)
		{
			try 
			{
                Console.WriteLine("@ GameMsg6 >> " + msg);
			/*	string[] cmds = msg.Split(':');
	            switch (cmds[0])
	            {
					case "dp_start": game.GameStart(cmds[1]); break;
					case "dp_player": game.SetRevalInfos(cmds[1]); break;
					case "dp_slot": game.FillSlot(cmds[1]); break;
					case "dp_gameover": game.DP_GameOver(cmds[1]); break;
					case "dp_draw": game.DP_Draw(cmds[1]); break;
					case "dp_timeup": game.DP_TiemUP(cmds[1]); break;
					case "dp_sync" : game.TimerSynchronization(cmds[1], delay); break;
	            }
	            */
	        }
	        catch (Exception e) { Console.WriteLine("GameMessageIn Exception:\r\n" + e.ToString()); }
		}

		

		void dpRoom(string msg)
		{
			string[] m = msg.Split('/');
			int sid = int.Parse(m[0]);
			string code = m[1];
			string account = m[2];

			/* find user in our list */
			for (int i =0; i< m_user.Count; i++)
            {
				if (m_user[i].account == account)
				{
					if(m_user[i].matchinfo.matchCode != code)
						return;
					if(m_user[i].sn != null) 
					{			
						//m_user[i].sn.Leave(CB_LeaveScene, sid);
						Console.WriteLine("Sn is not null");			
					}
					else 
					{	
						Console.WriteLine("create i = " + i);
						m_user[i].sn = new ArcaletScene(m_user[i].ag, sguid_game, sid);
						switch (i)
						{
							case 0:
								m_user[i].sn.onMessageIn += GameMessageIn1;
								m_user[i].sn.onCompletion += CB_EnterRoom1;
								break;
							case 1:
								m_user[i].sn.onMessageIn += GameMessageIn2;
								m_user[i].sn.onCompletion += CB_EnterRoom2;
								break;
							case 2:
								m_user[i].sn.onMessageIn += GameMessageIn3;
								m_user[i].sn.onCompletion += CB_EnterRoom3;
								break;	
							case 3:
								m_user[i].sn.onMessageIn += GameMessageIn4;
								m_user[i].sn.onCompletion += CB_EnterRoom4;
								break;
							case 4:
								m_user[i].sn.onMessageIn += GameMessageIn5;
								m_user[i].sn.onCompletion += CB_EnterRoom5;
								break;	
							case 5:
								m_user[i].sn.onMessageIn += GameMessageIn6;
								m_user[i].sn.onCompletion += CB_EnterRoom6;
								break;
						}
						
						m_user[i].sn.Launch();
					}
				}
			}
		}

		void CB_LeaveScene(int code, object token)
		{
			/*if(code == 0) {			
				Debug.Log("CB_LeaveScene Successed");			
				int sid = (int)token;
				sn = new ArcaletScene(ag, sguid_game, sid);
				sn.onMessageIn += GameMessageIn;
				sn.onCompletion += CB_EnterRoom;
				sn.Launch();
			}
			else {
				Debug.Log("CB_LeaveScene Failed: " + code);
				matchInfos.matchCode = "";
				ag.PrivacySend("cancel:" + ag.poid, serverSettings.dpPoid);
				Application.LoadLevel("MainMenu");
			}*/
		}	
	
		void CB_EnterRoom1(int code, ArcaletScene scene)
		{
			if(code == 0) {
                Console.WriteLine("CB_EnterRoom1 Successed");
				m_user[0].sn.Send("bbready:" + m_user[0].account + "/" + m_user[0].ag.poid);
			}
			else {
                Console.WriteLine("CB_EnterRoom1 Failed: " + code);
			}
		}

		void CB_EnterRoom2(int code, ArcaletScene scene)
		{
			if(code == 0) {
                Console.WriteLine("CB_EnterRoom2 Successed");
				m_user[1].sn.Send("bbready:" + m_user[1].account + "/" + m_user[1].ag.poid);
			}
			else {
                Console.WriteLine("CB_EnterRoom2 Failed: " + code);
			}
		}

		void CB_EnterRoom3(int code, ArcaletScene scene)
		{
			if(code == 0) {
                Console.WriteLine("CB_EnterRoom3 Successed");
				m_user[2].sn.Send("bbready:" + m_user[2].account + "/" + m_user[2].ag.poid);
			}
			else {
                Console.WriteLine("CB_EnterRoom3 Failed: " + code);
			}
		}

		void CB_EnterRoom4(int code, ArcaletScene scene)
		{
			if(code == 0) {
                Console.WriteLine("CB_EnterRoom4 Successed");
				m_user[3].sn.Send("bbready:" + m_user[3].account + "/" + m_user[3].ag.poid);
			}
			else {
                Console.WriteLine("CB_EnterRoom4 Failed: " + code);
			}
		}

		void CB_EnterRoom5(int code, ArcaletScene scene)
		{
			if(code == 0) {
                Console.WriteLine("CB_EnterRoom5 Successed");
				m_user[4].sn.Send("bbready:" + m_user[4].account + "/" + m_user[4].ag.poid);
			}
			else {
                Console.WriteLine("CB_EnterRoom5 Failed: " + code);
			}
		}

		void CB_EnterRoom6(int code, ArcaletScene scene)
		{
			if(code == 0) {
                Console.WriteLine("CB_EnterRoom6 Successed");
				m_user[5].sn.Send("bbready:" + m_user[5].account + "/" + m_user[5].ag.poid);
			}
			else {
                Console.WriteLine("CB_EnterRoom6 Failed: " + code);
			}
		}
		
        void setdpPoid(int dpPoid)
        {
            for (int i =0; i< m_user.Count; i++)
            {
                m_user[i].dpPoid = dpPoid;
            }
        }
    }

    internal class UserModel
    {
        internal MatchInfo matchinfo = null;
        internal string account = "";
        internal int poid = 0;
        internal int dpPoid = 0;
        internal ArcaletGame ag = null;
        internal Label userStatus = null;
        internal Label userName = null;
        internal Button matchButton = null;
		internal Button upButton = null;
		internal ArcaletScene sn = null;
    }
}

[System.Serializable]
public class MatchInfo
{
    public string matchCode = "";

    internal void GenerateMatchCode()
    {
        string code = "";
        char[] ch = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'R', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        Random crandom = new Random();
        do
        {
            for (int i = 0; i < 12; i++)
            {
                code += ch[crandom.Next(0, 36)].ToString();
            }
        }
        while (matchCode == code);
        matchCode = code;
    }
}

