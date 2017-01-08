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

        List<UserModel> m_user = new List<UserModel>();


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
                        m_user[i].matchButton.Click += new EventHandler(btnDemo_Click);
                        m_user[i].matchButton.Location = new Point(300, 15 + (i + 1) * 30);
                        this.Controls.Add(m_user[i].matchButton);

                        //StartCoroutine("DPLinkTimer");
                        m_user[i].ag.SendOnClose("quit:" + m_user[i].ag.gameUserid + "/" + m_user[i].ag.poid);
                        m_user[i].ag.Send("new:" + m_user[i].ag.gameUserid + "/" + m_user[i].ag.poid);

                        m_user[i].matchinfo = new MatchInfo();
                        m_user[i].matchinfo.GenerateMatchCode();
                    }
                }
            }
            else
            {
                Console.WriteLine("GG code = " + code);
            }
        }

        void btnDemo_Click(object sender, EventArgs e)
        {
            Button tmp = (Button)sender;
            for (int i = 0; i < m_user.Count; i++)
            {
                if (m_user[i].account == tmp.Name)
                {
                    string msg = m_user[i].ag.gameUserid + "/" + m_user[i].ag.poid + "/" + "test520" + "/" + m_user[i].matchinfo.matchCode;
                    m_user[i].ag.PrivacySend("match:" + msg, m_user[i].dpPoid);
                }
            }
        }

        void login()
        {
            for (int i= 1; i < 11; i++)
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
                        //    case "cancel": CancelMatchCheck(int.Parse(cmds[1])); break;
                }
            }
            catch (Exception) { }
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
        internal string matchCode = "";
        internal ArcaletGame ag = null;
        internal Label userStatus = null;
        internal Label userName = null;
        internal Button matchButton = null;
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

