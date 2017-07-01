using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using 标签打印及人员打印.Domain;

namespace 标签打印及人员打印
{
    public partial class Form1 : Form
    {
        #region 初始化工作
        const uint IMAGE_BITMAP = 0;
        const uint LR_LOADFROMFILE = 16;
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr LoadImage(IntPtr hinst, string lpszName, uint uType,
           int cxDesired, int cyDesired, uint fuLoad);
        [DllImport("Gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int DeleteObject(IntPtr ho);
        const string szSavePath = "C:\\Argox";
        const string szSaveFile = "C:\\Argox\\PPLB_Example.Prn";
        const string sznop1 = "nop_front\r\n";
        const string sznop2 = "nop_middle\r\n";
        [DllImport("Winpplb.dll")]
        private static extern int B_Bar2d_Maxi(int x, int y, int cl, int cc, int pc, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Bar2d_PDF417(int x, int y, int w, int v, int s, int c, int px,
            int py, int r, int l, int t, int o, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Bar2d_PDF417_N(int x, int y, int w, int h, string pParameter, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Bar2d_DataMatrix(int x, int y, int r, int l, int h, int v, string data);
        [DllImport("Winpplb.dll")]
        private static extern void B_ClosePrn();
        [DllImport("Winpplb.dll")]
        private static extern int B_CreatePrn(int selection, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Del_Form(string formname);
        [DllImport("Winpplb.dll")]
        private static extern int B_Del_Pcx(string pcxname);
        [DllImport("Winpplb.dll")]
        private static extern int B_Draw_Box(int x, int y, int thickness, int hor_dots,
            int ver_dots);
        [DllImport("Winpplb.dll")]
        private static extern int B_Draw_Line(char mode, int x, int y, int hor_dots, int ver_dots);
        [DllImport("Winpplb.dll")]
        private static extern int B_Error_Reporting(char option);
        [DllImport("Winpplb.dll")]
        private static extern IntPtr B_Get_DLL_Version(int nShowMessage);
        [DllImport("Winpplb.dll")]
        private static extern int B_Get_DLL_VersionA(int nShowMessage);
        [DllImport("Winpplb.dll")]
        private static extern int B_Get_Graphic_ColorBMP(int x, int y, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Get_Graphic_ColorBMPEx(int x, int y, int nWidth, int nHeight,
            int rotate, string id_name, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Get_Graphic_ColorBMP_HBitmap(int x, int y, int nWidth, int nHeight,
           int rotate, string id_name, IntPtr hbm);
        [DllImport("Winpplb.dll")]
        private static extern int B_Get_Pcx(int x, int y, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Initial_Setting(int Type, string Source);
        [DllImport("Winpplb.dll")]
        private static extern int B_WriteData(int IsImmediate, byte[] pbuf, int length);
        [DllImport("Winpplb.dll")]
        private static extern int B_ReadData(byte[] pbuf, int length, int dwTimeoutms);
        [DllImport("Winpplb.dll")]
        private static extern int B_Load_Pcx(int x, int y, string pcxname);
        [DllImport("Winpplb.dll")]
        private static extern int B_Open_ChineseFont(string path);
        [DllImport("Winpplb.dll")]
        private static extern int B_Print_Form(int labset, int copies, string form_out, string var);
        [DllImport("Winpplb.dll")]
        private static extern int B_Print_MCopy(int labset, int copies);
        [DllImport("Winpplb.dll")]
        private static extern int B_Print_Out(int labset);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Barcode(int x, int y, int ori, string type, int narrow,
            int width, int height, char human, string data);
        [DllImport("Winpplb.dll")]
        private static extern void B_Prn_Configuration();
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text(int x, int y, int ori, int font, int hor_factor,
            int ver_factor, char mode, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text_Chinese(int x, int y, int fonttype, string id_name,
            string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text_TrueType(int x, int y, int FSize, string FType,
            int Fspin, int FWeight, int FItalic, int FUnline, int FStrikeOut, string id_name,
            string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text_TrueType_W(int x, int y, int FHeight, int FWidth,
            string FType, int Fspin, int FWeight, int FItalic, int FUnline, int FStrikeOut,
            string id_name, string data);
        [DllImport("Winpplb.dll")]
        private static extern int B_Select_Option(int option);
        [DllImport("Winpplb.dll")]
        private static extern int B_Select_Option2(int option, int p);
        [DllImport("Winpplb.dll")]
        private static extern int B_Select_Symbol(int num_bit, int symbol, int country);
        [DllImport("Winpplb.dll")]
        private static extern int B_Select_Symbol2(int num_bit, string csymbol, int country);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Backfeed(char option);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Backfeed_Offset(int offset);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_CutPeel_Offset(int offset);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_BMPSave(int nSave, string strBMPFName);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Darkness(int darkness);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_DebugDialog(int nEnable);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Direction(char direction);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Form(string formfile);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Labgap(int lablength, int gaplength);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Labwidth(int labwidth);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Originpoint(int hor, int ver);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Prncomport(int baud, char parity, int data, int stop);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Prncomport_PC(int nBaudRate, int nByteSize, int nParity,
            int nStopBits, int nDsr, int nCts, int nXonXoff);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_Speed(int speed);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_ProcessDlg(int nShow);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_ErrorDlg(int nShow);
        [DllImport("Winpplb.dll")]
        private static extern int B_GetUSBBufferLen();
        [DllImport("Winpplb.dll")]
        private static extern int B_EnumUSB(byte[] buf);
        [DllImport("Winpplb.dll")]
        private static extern int B_CreateUSBPort(int nPort);
        [DllImport("Winpplb.dll")]
        private static extern int B_ResetPrinter();
        [DllImport("Winpplb.dll")]
        private static extern int B_GetPrinterResponse(byte[] buf, int nMax);
        [DllImport("Winpplb.dll")]
        private static extern int B_TFeedMode(int nMode);
        [DllImport("Winpplb.dll")]
        private static extern int B_TFeedTest();
        [DllImport("Winpplb.dll")]
        private static extern int B_CreatePort(int nPortType, int nPort, string filename);
        [DllImport("Winpplb.dll")]
        private static extern int B_Execute_Form(string form_out, string var);
        [DllImport("Winpplb.dll")]
        private static extern int B_Bar2d_QR(int x, int y, int model, int scl, char error,
            char dinput, int c, int d, int p, byte[] bytes);
        [DllImport("Winpplb.dll")]
        private static extern int B_GetNetPrinterBufferLen();
        [DllImport("Winpplb.dll")]
        private static extern int B_EnumNetPrinter(byte[] buf);
        [DllImport("Winpplb.dll")]
        private static extern int B_CreateNetPort(int nPort);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text_TrueType_Uni(int x, int y, int FSize, string FType,
            int Fspin, int FWeight, int FItalic, int FUnline, int FStrikeOut, string id_name,
            byte[] data, int format);
        [DllImport("Winpplb.dll")]
        private static extern int B_Prn_Text_TrueType_UniB(int x, int y, int FSize, string FType,
            int Fspin, int FWeight, int FItalic, int FUnline, int FStrikeOut, string id_name,
            byte[] data, int format);
        [DllImport("Winpplb.dll")]
        private static extern int B_GetUSBDeviceInfo(int nPort, byte[] pDeviceName,
            out int pDeviceNameLen, byte[] pDevicePath, out int pDevicePathLen);
        [DllImport("Winpplb.dll")]
        private static extern int B_Set_EncryptionKey(string encryptionKey);
        [DllImport("Winpplb.dll")]
        private static extern int B_Check_EncryptionKey(string decodeKey, string encryptionKey,
            int dwTimeoutms);

        System.Text.Encoding encAscII = System.Text.Encoding.ASCII;
        System.Text.Encoding encUnicode = System.Text.Encoding.Unicode;
        #endregion
        public Form1()
        {
            InitializeComponent();

            #region 数据组织
            //对数据进行组织
            //            SamProject samProject1 = new SamProject();
            //            samProject1.Name = "项目1";
            //            samProject1.Container = "500ML塑料瓶";
            //            samProject1.Reagent = "盐酸";
            //            samProject1.Count = 1;
            //
            //            SamProject samProject2 = new SamProject();
            //            samProject2.Name = "项目2";
            //            samProject2.Container = "250ML灭菌瓶一个";
            //
            //            SamProject samProject3 = new SamProject();
            //            samProject3.Name = "项目3";
            //            samProject3.Container = "250ML灭菌瓶一个";
            //            samProject3.Reagent = "其他";
            //
            //            SamProject samProject4 = new SamProject();
            //            samProject4.Name = "项目4";
            //            samProject4.Container = "500ML塑料瓶";
            //            samProject4.Reagent = "碱";
            //            samProject4.Count = 2;
            //
            //            SamProject samProject5 = new SamProject();
            //            samProject5.Name = "项目5";
            //            samProject5.Container = "500ML塑料瓶";
            //            samProject5.Reagent = "硝酸";
            //
            //            SamProject samProject6 = new SamProject();
            //            samProject6.Name = "项目6";
            //            samProject6.SequenceNumber = "VOC";
            //            samProject6.Container = "500ML塑料瓶";
            //
            //
            //            samProjects = new List<SamProject>();
            //            samProjects.Add(samProject1);
            //            samProjects.Add(samProject2);
            //            samProjects.Add(samProject3);
            //            samProjects.Add(samProject4);
            //            samProjects.Add(samProject5);
            //            samProjects.Add(samProject6);
            //
            //            string json1 = JsonConvert.SerializeObject(samProjects, Formatting.Indented);
            //            Console.WriteLine(json1); 
            #endregion

            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            var readFile = ReadFile(currentDirectory + Path.DirectorySeparatorChar + "samdata.json");
            var json = System.Text.Encoding.UTF8.GetString(readFile);
            Console.WriteLine("数据时：" + json);
            List<SamProject> samProjects = JsonConvert.DeserializeObject<List<SamProject>>(json);
            dgSamData.AutoGenerateColumns = false;
            dgSamData.DataSource = samProjects;
            Column1.FalseValue = 0;
            Column1.TrueValue = 1;
            Column2.DataPropertyName = "SequenceNumber";
            Column3.DataPropertyName = "Name";
            Column4.DataPropertyName = "Reagent";
            Column5.DataPropertyName = "Container";

            Column6.DataPropertyName = "Count";
        }

        //代表这是第几份
        private int printNum = 1;
        
        private void PrintEvent()
        {
            string samCode = tbSamCode.Text;
            if (samCode.Equals(""))
            {
                MessageBox.Show("请输入采样编号");
                return;
            }

            string company = tbCompany.Text;
            if (company.Equals(""))
            {
                MessageBox.Show("请输入检测公司名称");
                return;
            }

            //准备要打印的数据,首先计算一下选中的总数量
            int rowsCount = dgSamData.Rows.Count;
            int count = 0;
            for (int i = 0; i < rowsCount; i++)
            {
                //                DataGridViewCheckBoxCell dataGridViewCheckBoxCell = ((DataGridViewCheckBoxCell) dgSamData.Rows[i].Cells["Column1"]);
                //                if (dataGridViewCheckBoxCell.Value.Equals("true"))
                //                {
                //                    MessageBox.Show("有选中的，第" + i + "行");
                //
                //                }
                DataGridViewCheckBoxCell dataGridViewCheckBoxCell =
                    ((DataGridViewCheckBoxCell)dgSamData.Rows[i].Cells["Column1"]);
                if (dataGridViewCheckBoxCell.Value == dataGridViewCheckBoxCell.TrueValue)
                {
                    DataGridViewTextBoxCell countTextBoxCell =
                        (DataGridViewTextBoxCell)dgSamData.Rows[i].Cells["Column6"];
                    Debug.Assert(countTextBoxCell != null, "countTextBoxCell != null");
                    count += (int)countTextBoxCell.Value;
                    Console.WriteLine("选中了第" + i + "行,数量是：" + countTextBoxCell.Value + "总数量是：" + count);
                    Console.WriteLine("------------");
                }
            }
            //初始化打印环境
            initialPrint();
            for (int i = 0; i < rowsCount; i++)
            {
                DataGridViewCheckBoxCell dataGridViewCheckBoxCell =
                    ((DataGridViewCheckBoxCell)dgSamData.Rows[i].Cells["Column1"]);
                if (dataGridViewCheckBoxCell.Value == dataGridViewCheckBoxCell.TrueValue)
                {
                    //这里进行选中对象转换
                    DataGridViewTextBoxCell countTextBoxCell =
                        (DataGridViewTextBoxCell)dgSamData.Rows[i].Cells["Column6"];
                    //这是要循环打印的次数
                    int printCount = (int)countTextBoxCell.Value;
                    for (int j = 0; j < printCount; j++)
                    {
                        PrintObject printObject = new PrintObject();
                        printObject.Company = company;
                        printObject.Container = (string)dgSamData.Rows[i].Cells["Column5"].Value;
                        //取得分组编号单元格
                        DataGridViewTextBoxCell dataGridViewTextBoxCell =
                            (DataGridViewTextBoxCell)dgSamData.Rows[i].Cells["Column2"];
                        if (dataGridViewTextBoxCell.Value == null)
                        {
                            DataGridViewTextBoxCell nameViewTextBoxCell =
                                (DataGridViewTextBoxCell)dgSamData.Rows[i].Cells["Column3"];
                            printObject.Name = (string)nameViewTextBoxCell.Value;
                        }
                        else
                        {
                            printObject.Name = (string)dataGridViewTextBoxCell.Value;
                        }

                        printObject.Reagent = (string)dgSamData.Rows[i].Cells["Column4"].Value;
//                        if (printCount > 1)
//                        {
                            printObject.SamCode = samCode + "-" + printNum;
//                        }
//                        else
//                        {
//                            printObject.SamCode = samCode;
//                        }

                        printObject.QrCode = samCode + "#" + printNum + "#" + count;
                        printObject.Time = DateTime.Now.ToLongDateString();
                        print(printObject);
                        printNum++;
                    }
                }
            }
            B_ClosePrn();
            printNum = 1;
        }

        private void initialPrint()
        {
            //Test code start
            // open port.
            int nLen, ret, sw;
            byte[] pbuf = new byte[128];
            string strmsg;
            IntPtr ver;
            // dll version.
            ver = B_Get_DLL_Version(0);

            // search port.
            nLen = B_GetUSBBufferLen() + 1;
            strmsg = "DLL ";
            strmsg += Marshal.PtrToStringAnsi(ver);
            strmsg += "\r\n";
            if (nLen > 1)
            {
                byte[] buf1, buf2;
                int len1 = 128, len2 = 128;
                buf1 = new byte[len1];
                buf2 = new byte[len2];
                B_EnumUSB(pbuf);
                B_GetUSBDeviceInfo(1, buf1, out len1, buf2, out len2);
                sw = 1;
                if (1 == sw)
                {
                    ret = B_CreatePrn(12, encAscII.GetString(buf2, 0, len2));// open usb.
                }
                else
                {
                    ret = B_CreateUSBPort(1);// must call B_GetUSBBufferLen() function fisrt.
                }
                if (0 != ret)
                {
                    strmsg += "Open USB fail!";
                }
                else
                {
                    strmsg += "Open USB:\r\nDevice name: ";
                    strmsg += encAscII.GetString(buf1, 0, len1);
                    strmsg += "\r\nDevice path: ";
                    strmsg += encAscII.GetString(buf2, 0, len2);
                    //sw = 2;
                    if (2 == sw)
                    {
                        //Immediate Error Report.
                        B_WriteData(1, encAscII.GetBytes("^ee\r\n"), 5);//^ee
                        ret = B_ReadData(pbuf, 4, 1000);
                    }
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(szSavePath);
                ret = B_CreatePrn(0, szSaveFile);// open file.
                strmsg += "Open ";
                strmsg += szSaveFile;
                if (0 != ret)
                {
                    strmsg += " file fail!";
                }
                else
                {
                    strmsg += " file succeed!";
                }
            }
            //MessageBox.Show(strmsg);
            if (0 != ret)
                return;
            if (0 != ret)
                return;
            // sample setting.
            //设定出错环境
            B_Set_DebugDialog(0);
            B_Set_Originpoint(0, 0);
            B_Select_Option(2);
            B_Set_Darkness(8);
            B_Del_Pcx("*");// delete all picture.
            B_WriteData(0, encAscII.GetBytes(sznop2), sznop2.Length);
            B_WriteData(1, encAscII.GetBytes(sznop1), sznop1.Length);

        }

        private void print(PrintObject printObject)
        {
            ////draw box.

            B_Draw_Box(60, 60, 6, 1150, 650);
            B_Prn_Text_TrueType_Uni(100, 100, 50, "Times New Roman", 1, 400, 0, 0, 0, "A1", Encoding.Unicode.GetBytes("样品编号："), 1);//UTF-16
            B_Prn_Text_TrueType_Uni(300, 100, 50, "Times New Roman", 1, 400, 0, 0, 0, "A2", Encoding.Unicode.GetBytes(printObject.SamCode), 1);//UTF-16
            ////划线
            B_Draw_Line('O', 300, 160, 300, 4);
            B_Prn_Text_TrueType_Uni(600, 100, 50, "Times New Roman", 1, 400, 0, 0, 0, "A3", Encoding.Unicode.GetBytes("□待测   □在侧   □已测"), 1);//UTF-16
            B_Prn_Text_TrueType_Uni(100, 200, 50, "Times New Roman", 1, 400, 0, 0, 0, "B1", Encoding.Unicode.GetBytes("监测项目:"), 1);//UTF-16
            B_Prn_Text_TrueType_Uni(300, 200, 50, "Times New Roman", 1, 400, 0, 0, 0, "B2", Encoding.Unicode.GetBytes(printObject.Name), 1);//UTF-16
            //////划线
            B_Draw_Line('O', 300, 260, 850, 4);
            //if (reagent.)
            if (printObject.Reagent.Equals("硝酸"))
            {
                B_Prn_Text_TrueType_Uni(80, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C7", Encoding.Unicode.GetBytes("■"), 1);//UTF-16
            }
            else
            {
                B_Prn_Text_TrueType_Uni(80, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C0", Encoding.Unicode.GetBytes("□"), 1);//UTF-16
            }
            B_Prn_Text_TrueType_Uni(100, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C1", Encoding.Unicode.GetBytes("硝酸"), 1);//UTF-16
            if (printObject.Reagent.Equals("硫酸"))
            {
                B_Prn_Text_TrueType_Uni(280, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C7", Encoding.Unicode.GetBytes("■"), 1);//UTF-16
            }
            else
            {
                B_Prn_Text_TrueType_Uni(280, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C0", Encoding.Unicode.GetBytes("□"), 1);//UTF-16
            }
            B_Prn_Text_TrueType_Uni(300, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C2", Encoding.Unicode.GetBytes("硫酸"), 1);//UTF-16
            if (printObject.Reagent.Equals("盐酸"))
            {
                B_Prn_Text_TrueType_Uni(480, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C7", Encoding.Unicode.GetBytes("■"), 1);//UTF-16
            }
            else
            {
                B_Prn_Text_TrueType_Uni(480, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C0", Encoding.Unicode.GetBytes("□"), 1);//UTF-16
            }
            B_Prn_Text_TrueType_Uni(500, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C3", Encoding.Unicode.GetBytes("盐酸"), 1);//UTF-16
            if (printObject.Reagent.Equals("磷酸"))
            {
                B_Prn_Text_TrueType_Uni(680, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C7", Encoding.Unicode.GetBytes("■"), 1);//UTF-16
            }
            else
            {
                B_Prn_Text_TrueType_Uni(680, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C0", Encoding.Unicode.GetBytes("□"), 1);//UTF-16
            }
            B_Prn_Text_TrueType_Uni(700, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C4", Encoding.Unicode.GetBytes("磷酸"), 1);//UTF-16
            if (printObject.Reagent.Equals("碱"))
            {

                B_Prn_Text_TrueType_Uni(880, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C7", Encoding.Unicode.GetBytes("■"), 1); //UTF-16
            }
            else
            {

                B_Prn_Text_TrueType_Uni(880, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C0", Encoding.Unicode.GetBytes("□"), 1);//UTF-16
            }
            B_Prn_Text_TrueType_Uni(900, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C5", Encoding.Unicode.GetBytes("碱"), 1);//UTF-16
            if (printObject.Reagent.Equals("其他"))
            {
                B_Prn_Text_TrueType_Uni(1030, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C7", Encoding.Unicode.GetBytes("■"), 1); //UTF-16
            }
            else
            {
                B_Prn_Text_TrueType_Uni(1030, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C0", Encoding.Unicode.GetBytes("□"), 1);//UTF-16
            }
            B_Prn_Text_TrueType_Uni(1050, 300, 50, "Times New Roman", 1, 400, 0, 0, 0, "C6", Encoding.Unicode.GetBytes("其他"), 1);//UTF-16
            B_Prn_Text_TrueType_Uni(350, 370, 50, "Times New Roman", 1, 400, 0, 0, 0, "d1", Encoding.Unicode.GetBytes(printObject.Container), 1);//UTF-16
            B_Prn_Text_TrueType_Uni(350, 450, 50, "Times New Roman", 1, 400, 0, 0, 0, "E1", Encoding.Unicode.GetBytes(printObject.Time), 1);//UTF-16
            B_Prn_Text_TrueType_Uni(350, 550, 50, "Times New Roman", 1, 400, 0, 0, 0, "F1", Encoding.Unicode.GetBytes(printObject.Company), 1);//UTF-16
            B_Bar2d_QR(900, 400, 1, 11, 'M', 'A', 0, 0, 0, Encoding.UTF8.GetBytes(printObject.QrCode));

            // output.
            B_Print_Out(1);// copy 2.
        }


        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        private void 标签打印_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PrintEvent();
            }
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            PrintEvent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string content = tbName.Text.Trim();
            if (content.Equals(null) || content.Equals(""))
            {
                MessageBox.Show("请输入要打印的内容");
                return;
            }
//            string utf8_string = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(content));
//            Console.WriteLine(utf8_string);
            //初始化打印环境
            initialPrint();
            B_Bar2d_QR(400, 200, 1, 20, 'M', 'A', 0, 0, 0, Encoding.UTF8.GetBytes(content));
            // output.
            B_Print_Out(1);// copy 2.
            B_ClosePrn();
        }
    }
}
