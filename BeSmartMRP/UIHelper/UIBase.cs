
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace BeSmartMRP.UIHelper
{
    public class UIBase
    {

        private static bool stop = false;
        private static Thread thread;

        private static DevExpress.Utils.WaitDialogForm dlgWaitWind = null;
        private static void pmCreateWaitDialog()
        {
            dlgWaitWind = new DevExpress.Utils.WaitDialogForm("Loading Components...");
        }

        public static void WaitWind(string inText)
        {
            if (dlgWaitWind == null)
            {
                pmCreateWaitDialog();
            }

            //stop = false;
            //thread = new Thread(new ThreadStart(ptStartProcess));
            //thread.Start();
            
            dlgWaitWind.Caption = inText;

        }

        public static void WaitClear()
        {
            //if (dlgWaitWind != null) 
             //   dlgWaitWind.Close();

            //ptEndProcess();
            dlgWaitWind.Hide();
        }

        private static void ptStartProcess()
        {
            Thread.Sleep(400);
            if (stop)
                return;

            dlgWaitWind.Show();
            
            try
            {
                while (!stop)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);
                }
            }
            catch
            {
            }
        }

        private static void ptEndProcess()
        {
            stop = true;
            thread.Join();
        }

        public static string GetAppText(AppLocale inLocale, string[] inText)
        {
            return pmGetAppText(inLocale, inText);
        }

        public static string GetAppUIText(string[] inText)
        {
            return pmGetAppText(App.mLocale_UI, inText);
        }

        public static string GetAppReportText(string[] inText)
        {
            return pmGetAppText(App.mLocale_Report, inText);
        }

        private static string pmGetAppText(AppLocale inLocale, string[] inText)
        {
            string strText = "";
            switch (inLocale)
            {
                case AppLocale.th_TH:
                    strText = (inText.Length > 0 ? inText[0] : "");
                    break;
                case AppLocale.en_US:
                    strText = (inText.Length > 1 ? inText[1] : "");
                    break;
                default:
                    strText = inText[0];
                    break;
            }

            return strText;
        }

        public static string GetThaiMonth(int inMth)
        {
            string[] aMth1 = new string[] { "ม.ค.", "ก.พ.", "มี.ค.", "เม.ย.", "พ.ค.", "มิ.ย.", "ก.ค.", "ส.ค.", "ก.ย.", "ต.ค.", "พ.ย.", "ธ.ค." };
            return aMth1[inMth - 1];
        }

        public static string GetThaiMonthLong(int inMth)
        {
            string[] aMth1 = new string[] { "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม" };
            return aMth1[inMth - 1];
        }

        public static string GetThaiDate(DateTime inDate, string inFormat)
        {
            string strDate = inFormat;
            //string[] aMth1 = new string[] { "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม" };
            //return aMth1[inMth - 1];
            strDate = strDate.Replace("yyyy", (inDate.Year + 543).ToString("0000"));
            strDate = strDate.Replace("yy", (inDate.Year + 543).ToString("0000").Substring(2, 2));
            strDate = strDate.Replace("MMMM", GetThaiMonthLong(inDate.Month));
            strDate = strDate.Replace("MM", GetThaiMonth(inDate.Month));
            strDate = strDate.Replace("dd", inDate.Day.ToString("00"));
            strDate = strDate.Replace("d", inDate.Day.ToString());
            return strDate;
        }

        public static void CreateStandardToolbar(DevExpress.XtraBars.BarManager inBarManager, DevExpress.XtraBars.Bar inBar)
        {
            inBarManager.HideBarsWhenMerging = false;
            pmCreateStandardToolbar(inBarManager, inBar);
        }

        public static void CreateTransactionToolbar(DevExpress.XtraBars.BarManager inBarManager, DevExpress.XtraBars.Bar inBar)
        {
            inBarManager.HideBarsWhenMerging = false;
            pmCreateTransactionToolbar(inBarManager, inBar);
        }

        public static void CreatePopUpToolbar(DevExpress.XtraBars.BarManager inBarManager, DevExpress.XtraBars.Bar inBar)
        {
            inBarManager.HideBarsWhenMerging = false;
            pmCreatePopUpToolbar(inBarManager, inBar);
        }

        private static void pmCreateStandardToolbar(DevExpress.XtraBars.BarManager inBarManager, DevExpress.XtraBars.Bar inBar)
        {
            DevExpress.XtraBars.BarButtonItem oEnter = pmCreateBar(WsToolBar.Enter.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "เลือก", "Select" }), global::BeSmartMRP.Properties.Resources.text_rich, Keys.Enter);
            DevExpress.XtraBars.BarButtonItem oAdd = pmCreateBar(WsToolBar.Insert.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "เพิ่ม", "Add" }), global::BeSmartMRP.Properties.Resources.NewDoc2, Keys.F3);
            DevExpress.XtraBars.BarButtonItem oEdit = pmCreateBar(WsToolBar.Update.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "แก้ไข", "Edit" }), global::BeSmartMRP.Properties.Resources.Open, Keys.F2);
            DevExpress.XtraBars.BarButtonItem oDelete = pmCreateBar(WsToolBar.Delete.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "ลบข้อมูล", "Delete" }), global::BeSmartMRP.Properties.Resources.DelItem, Keys.F4);
            DevExpress.XtraBars.BarButtonItem oSearch = pmCreateBar(WsToolBar.Search.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "ค้นหา", "Search" }), global::BeSmartMRP.Properties.Resources.Search, Keys.F6);
            DevExpress.XtraBars.BarButtonItem oPrint = pmCreateBar(WsToolBar.Print.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "พิมพ์", "Print" }), global::BeSmartMRP.Properties.Resources.Print2, Keys.F5);
            DevExpress.XtraBars.BarButtonItem oSave = pmCreateBar(WsToolBar.Save.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "บันทึก", "Save" }), global::BeSmartMRP.Properties.Resources.Save, Keys.F10);
            DevExpress.XtraBars.BarButtonItem oCancel = pmCreateBar(WsToolBar.Undo.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "ออก", "Esc" }), global::BeSmartMRP.Properties.Resources.Undo, Keys.Cancel);
            //DevExpress.XtraBars.BarButtonItem oCancel = pmCreateBar(WsToolBar.Undo.ToString(), "ยกเลิก", global::BeSmartMRP.Properties.Resources.Undo, Keys.Cancel);
            DevExpress.XtraBars.BarButtonItem oRefresh = pmCreateBar(WsToolBar.Refresh.ToString(), "Refresh", global::BeSmartMRP.Properties.Resources.Refresh2, Keys.F12);
            inBarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] { oEnter, oAdd, oEdit, oDelete, oPrint, oSearch, oSave, oCancel, oRefresh });

            oPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            oAdd.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F3-เพิ่มข้อมูล", "F3-Add Data" });
            oEdit.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F2-แก้ไขข้อมูล", "F2-Edit Data" });
            oDelete.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F4-ลบข้อมูล", "F4-Delete Data" });
            oPrint.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F5-พิมพ์เอกสาร", "F5-Print Data" });
            oSearch.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F6-ค้นหาข้อมูล", "F6-Search Data" });
            oSave.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F10-บันทึกข้อมูล", "F10-Save Data" });
            oRefresh.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F12-Refresh Data", "F12-Refresh Data" });

            inBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oEnter, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oAdd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oEdit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oDelete, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oPrint, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oSearch, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oCancel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oRefresh, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            });

        }

        private static void pmCreateTransactionToolbar(DevExpress.XtraBars.BarManager inBarManager, DevExpress.XtraBars.Bar inBar)
        {

            DevExpress.XtraBars.BarButtonItem oEnter = pmCreateBar(WsToolBar.Enter.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "เลือก", "Select" }), global::BeSmartMRP.Properties.Resources.text_rich, Keys.Enter);
            DevExpress.XtraBars.BarButtonItem oAdd = pmCreateBar(WsToolBar.Insert.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "เพิ่ม", "Add" }), global::BeSmartMRP.Properties.Resources.NewDoc2, Keys.F3);
            DevExpress.XtraBars.BarButtonItem oEdit = pmCreateBar(WsToolBar.Update.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "แก้ไข", "Edit" }), global::BeSmartMRP.Properties.Resources.Open, Keys.F2);
            DevExpress.XtraBars.BarButtonItem oDelete = pmCreateBar(WsToolBar.Delete.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "ลบข้อมูล", "Delete" }), global::BeSmartMRP.Properties.Resources.DelItem, Keys.F4);
            DevExpress.XtraBars.BarButtonItem oPrint = pmCreateBar(WsToolBar.Print.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "พิมพ์", "Print" }), global::BeSmartMRP.Properties.Resources.Print2, Keys.F5);
            DevExpress.XtraBars.BarButtonItem oCancelDoc = pmCreateBar(WsToolBar.Cancel.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "ยกเลิก", "Cancel" }), global::BeSmartMRP.Properties.Resources.CancelDoc, Keys.F7);
            DevExpress.XtraBars.BarButtonItem oCloseDoc = pmCreateBar(WsToolBar.Close.ToString(), "Close/UnClose", global::BeSmartMRP.Properties.Resources.folder_closed, Keys.F8);
            DevExpress.XtraBars.BarButtonItem oSearch = pmCreateBar(WsToolBar.Search.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "ค้นหา", "Search" }), global::BeSmartMRP.Properties.Resources.Search, Keys.F6);
            DevExpress.XtraBars.BarButtonItem oSave = pmCreateBar(WsToolBar.Save.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "บันทึก", "Save" }), global::BeSmartMRP.Properties.Resources.Save, Keys.F10);
            DevExpress.XtraBars.BarButtonItem oCancel = pmCreateBar(WsToolBar.Undo.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "ยกเลิก", "Esc" }), global::BeSmartMRP.Properties.Resources.Undo, Keys.Cancel);
            DevExpress.XtraBars.BarButtonItem oRefresh = pmCreateBar(WsToolBar.Refresh.ToString(), "Refresh", global::BeSmartMRP.Properties.Resources.Refresh2, Keys.F12);
            inBarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] { oEnter, oAdd, oEdit, oDelete, oPrint, oSearch, oSave, oCancelDoc, oCloseDoc, oCancel, oRefresh });

            oAdd.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F3-เพิ่มข้อมูล", "F3-Add Data" });
            oEdit.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F2-แก้ไขข้อมูล", "F2-Edit Data" });
            oDelete.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F4-ลบข้อมูล", "F4-Delete Data" });
            oPrint.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F5-พิมพ์เอกสาร", "F5-Print Data" });
            oSearch.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F6-ค้นหาข้อมูล", "F6-Search Data" });
            oSave.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F10-บันทึกข้อมูล", "F10-Save Data" });
            oRefresh.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "F12-Refresh Data", "F12-Refresh Data" });


            inBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oEnter, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oAdd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oEdit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oPrint, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oSearch, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oDelete, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oCancelDoc, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oCloseDoc, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oCancel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oRefresh, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            });

        }

        private static void pmCreatePopUpToolbar(DevExpress.XtraBars.BarManager inBarManager, DevExpress.XtraBars.Bar inBar)
        {
            DevExpress.XtraBars.BarButtonItem oEnter = pmCreateBar(WsToolBar.Enter.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "เลือก", "Select" }), global::BeSmartMRP.Properties.Resources.text_rich, Keys.Enter);
            DevExpress.XtraBars.BarButtonItem oExit = pmCreateBar(WsToolBar.Exit.ToString(), UIBase.GetAppText(App.mLocale_UI, new string[] { "ยกเลิก", "Esc" }), global::BeSmartMRP.Properties.Resources.Exit01, Keys.Escape);
            DevExpress.XtraBars.BarButtonItem oRefresh = pmCreateBar(WsToolBar.Refresh.ToString(), "Refresh", global::BeSmartMRP.Properties.Resources.Refresh2, Keys.F12);
            inBarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] { oEnter, oExit, oRefresh });

            oEnter.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "Enter-เลือก", "Enter-Select Data" });
            oExit.Hint = UIBase.GetAppText(App.mLocale_UI, new string[] { "Esc-ยกเลิก", "Esc-Cancel" });
            oRefresh.Hint = "F12-Refresh";

            inBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oEnter, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oExit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            ,new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, oRefresh, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)
            });

        }

        private static DevExpress.XtraBars.BarButtonItem pmCreateBar(string inName, string inText, System.Drawing.Image inGlyph, System.Windows.Forms.Keys inShortcut)
        {
            DevExpress.XtraBars.BarButtonItem barNewItem = new DevExpress.XtraBars.BarButtonItem();
            barNewItem.Caption = inText;
            barNewItem.Glyph = inGlyph;
            barNewItem.ItemShortcut = new DevExpress.XtraBars.BarShortcut(inShortcut);
            barNewItem.Tag = inName;
            barNewItem.Name = inName;
            barNewItem.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            return barNewItem;
        }

        public static void SetDefaultGridViewAppreance(DevExpress.XtraGrid.Views.Grid.GridView inSender)
        {

            if (inSender.OptionsBehavior.Editable == true)
            {
                inSender.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(255, 238, 153);
                inSender.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(255, 238, 153);
                inSender.Appearance.FocusedCell.Options.UseBackColor = true;
            }

        }

        public static void SetDefaultChildAppreance(Control inSender)
        {

            foreach (Control oSender in inSender.Controls)
            {

                if (oSender.Controls.Count > 1)
                {
                    SetDefaultChildAppreance(oSender);
                    if (oSender is DevExpress.XtraGrid.GridControl)
                    {
                        DevExpress.XtraGrid.GridControl oGrid = oSender as DevExpress.XtraGrid.GridControl;
                        if (oGrid.Views.Count > 0)
                        {
                            for (int i = 0; i < oGrid.Views.Count; i++)
                            {
                                DevExpress.XtraGrid.Views.Grid.GridView gv = oGrid.Views[i] as DevExpress.XtraGrid.Views.Grid.GridView;
                                if (gv != null)
                                {
                                    SetDefaultGridViewAppreance(gv);
                                }
                            }
                        }
                    }
                }
                else
                {

                    if (oSender.Parent is DevExpress.XtraEditors.CalcEdit
                        || oSender is DevExpress.XtraEditors.ButtonEdit
                        || oSender is DevExpress.XtraEditors.ComboBoxEdit
                        || oSender.Parent is DevExpress.XtraEditors.DateEdit
                        || oSender.Parent is DevExpress.XtraEditors.SpinEdit
                        || oSender.Parent.Parent is DevExpress.XtraGrid.GridControl)
                    {

                        DevExpress.XtraEditors.BaseEdit oSetCtrl = oSender as DevExpress.XtraEditors.BaseEdit;
                        if (oSetCtrl != null)
                        {
                            oSetCtrl.Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                            //oSetCtrl.Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular);
                            oSetCtrl.Properties.Appearance.BackColor = System.Drawing.Color.White;
                            //oSetCtrl.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Moccasin;
                            //oSetCtrl.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(197, 217, 241);  //Dark Blue
                            //oSetCtrl.Properties.AppearanceFocused.BackColor = System.Drawing.Color.LemonChiffon;
                            oSetCtrl.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(255, 238, 153);  //เขียวแบบ SAP

                            oSetCtrl.Properties.AppearanceDisabled.BackColor = System.Drawing.SystemColors.ActiveBorder;
                            oSetCtrl.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
                        
                        }

                        if (oSender.Parent is DevExpress.XtraEditors.CalcEdit
                            || oSender.Parent is DevExpress.XtraEditors.DateEdit
                            || oSender.Parent is DevExpress.XtraEditors.SpinEdit
                            || oSender is DevExpress.XtraEditors.ComboBoxEdit
                            || oSender is DevExpress.XtraEditors.CalcEdit
                            || oSender is DevExpress.XtraEditors.DateEdit
                            || oSender is DevExpress.XtraEditors.SpinEdit)                        
                        {
                            oSetCtrl.KeyDown += new KeyEventHandler(txtSpin_KeyDown);
                        }

                    }
                    else if (oSender is System.Windows.Forms.Label)
                    {
                        System.Windows.Forms.Label oSetCtrl = oSender as System.Windows.Forms.Label;
                        if (oSetCtrl != null)
                        {
                            bool bllIsUnderLine = oSetCtrl.Font.Underline;
                            bool bllIsItalic = oSetCtrl.Font.Italic;
                            bool bllIsStrikeout = oSetCtrl.Font.Strikeout;

                            System.Drawing.FontStyle fs = System.Drawing.FontStyle.Bold;
                            if (oSetCtrl.Font.Underline)
                            {
                                fs = fs | System.Drawing.FontStyle.Underline;
                            }
                            if (oSetCtrl.Font.Italic)
                            {
                                fs = fs | System.Drawing.FontStyle.Italic;
                            }
                            if (oSetCtrl.Font.Strikeout)
                            {
                                fs = fs | System.Drawing.FontStyle.Strikeout;
                            }

                            oSetCtrl.Font = new System.Drawing.Font("Tahoma", 10, fs);
                            //oSetCtrl.Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Regular);
                        }
                    }


                }

                //this.pgfMainEdit.AppearancePage.HeaderActive.BackColor = System.Drawing.Color.White;
                //this.pgfMainEdit.AppearancePage.HeaderActive.BackColor2 = System.Drawing.Color.SkyBlue;
                //this.pgfMainEdit.AppearancePage.HeaderActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
                //this.pgfMainEdit.AppearancePage.HeaderActive.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
                //this.pgfMainEdit.AppearancePage.HeaderActive.Options.UseBackColor = true;
                //this.pgfMainEdit.AppearancePage.HeaderActive.Options.UseFont = true;
                //this.pgfMainEdit.AppearancePage.PageClient.BackColor = System.Drawing.Color.SkyBlue;
                //this.pgfMainEdit.AppearancePage.PageClient.Options.UseBackColor = true;
                //this.pgfMainEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            }
        }

        private static void txtSpin_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            { 
                case Keys.Up:
                    if (sender is DevExpress.XtraEditors.ComboBoxEdit)
                    {
                        DevExpress.XtraEditors.ComboBoxEdit oSender = sender as DevExpress.XtraEditors.ComboBoxEdit;
                        if (oSender != null && oSender.IsPopupOpen == false)
                        {
                            System.Windows.Forms.SendKeys.Send("+{TAB}");
                            e.Handled = true;
                        }
                    }
                    else if (sender is DevExpress.XtraEditors.DateEdit)
                    {
                        DevExpress.XtraEditors.DateEdit oSender = sender as DevExpress.XtraEditors.DateEdit;
                        if (oSender != null && oSender.IsPopupOpen == false)
                        {
                            System.Windows.Forms.SendKeys.Send("+{TAB}");
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        System.Windows.Forms.SendKeys.Send("+{TAB}");
                        e.Handled = true;
                    }
                    break;
                case Keys.Down:
                    if (sender is DevExpress.XtraEditors.ComboBoxEdit)
                    {
                        DevExpress.XtraEditors.ComboBoxEdit oSender = sender as DevExpress.XtraEditors.ComboBoxEdit;
                        if (oSender != null && oSender.IsPopupOpen == false)
                        {
                            System.Windows.Forms.SendKeys.Send("{TAB}");
                            e.Handled = true;
                        }
                    }
                    else if (sender is DevExpress.XtraEditors.DateEdit)
                    {
                        DevExpress.XtraEditors.DateEdit oSender = sender as DevExpress.XtraEditors.DateEdit;
                        if (oSender != null && oSender.IsPopupOpen == false)
                        {
                            System.Windows.Forms.SendKeys.Send("{TAB}");
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        System.Windows.Forms.SendKeys.Send("{TAB}");
                        e.Handled = true;
                    }
                    break;
                case Keys.Delete:
                    if (sender is DevExpress.XtraEditors.DateEdit)
                    {
                        DevExpress.XtraEditors.DateEdit oSender = sender as DevExpress.XtraEditors.DateEdit;
                        if (oSender != null && oSender.Properties.AllowNullInput == DevExpress.Utils.DefaultBoolean.True)
                        {
                            oSender.EditValue = null;
                            e.Handled = true;
                        }
                    }
                    break;
            }

            //if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            //{
            //    switch (e.KeyCode)
            //    {
            //        case Keys.Up:
            //            System.Windows.Forms.SendKeys.Send("+{TAB}");
            //            break;
            //        case Keys.Down:
            //            System.Windows.Forms.SendKeys.Send("{TAB}");
            //            break;
            //    }
            //    e.Handled = true;
            //}

        }

        public static string GetCostText(string inType)
        {
            string strRetStr = "";
            if (System.IO.File.Exists(Application.StartupPath + "\\Support\\Costing.txt"))
            {
                string strFileName = Application.StartupPath + "\\Support\\Costing.txt";

                try
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(strFileName, System.Text.Encoding.Default);
                    string strLine = "";
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        string[] staText = strLine.Split("=".ToCharArray());
                        if (staText.Length > 0 && staText[0].TrimEnd() == inType)
                        {
                            strRetStr = staText[1].TrimEnd();
                            break;
                        }
                        
                    }
                    sr.Close();
                }
                catch { }

            }
            return strRetStr;
        }

        public static bool LoadComportConfig(string inFileConfig, ref string ioComport, ref string ioCashDrawerOpenCmd,ref string ioErrorMsg)
        {

            bool bllResult = false;
            string strFileName = inFileConfig;

            try
            {
                IniUtil ini = new IniUtil(strFileName);
                ioComport = ini.IniReadValue("CashDrawer", "COMPORT");
                ioCashDrawerOpenCmd = ini.IniReadValue("CashDrawer", "OpenCmd");
                bllResult = true;
            }
            catch (Exception ex)
            {
                ioErrorMsg = ex.Message;
            }
            finally
            { }
            return bllResult;
        }
    
    }


}
