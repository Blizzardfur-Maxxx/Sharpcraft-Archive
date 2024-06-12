using Microsoft.VisualBasic;
using SharpCraft.Client.Gamemode;
using SharpCraft.Client.Renderer;
using SharpCraft.Core.i18n;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SharpCraft.Client.GUI.Screens.Worldselect
{
    public class SelectWorldScreen : Screen
    {
        protected Screen parentScreen;
        protected string screenTitle = "Select world";
        private bool selected = false;
        private int selectedWorld;
        private List<LevelSummary> saveList;
        private AbstractSelectionList worldSlotContainer;
        private string field_22098_o;
        private string field_22097_p;
        private bool deleting;
        private Button buttonRename;
        private Button buttonSelect;
        private Button buttonDelete;
        public SelectWorldScreen(Screen guiScreen1)
        {
            parentScreen = guiScreen1;
        }

        class WorldSelectionList : AbstractSelectionList
        {
            readonly SelectWorldScreen SELF;
            public WorldSelectionList(SelectWorldScreen guiSelectWorld1) : base(guiSelectWorld1.mc, guiSelectWorld1.width, guiSelectWorld1.height, 32, guiSelectWorld1.height - 64, 36)
            {
                SELF = guiSelectWorld1;
            }

            protected override int GetSize()
            {
                return SELF.saveList.Count;
            }

            protected override void ElementClicked(int i1, bool z2)
            {
                SELF.selectedWorld = i1;
                bool z3 = SELF.selectedWorld >= 0 && SELF.selectedWorld < GetSize();
                SELF.buttonSelect.enabled = z3;
                SELF.buttonRename.enabled = z3;
                SELF.buttonDelete.enabled = z3;
                if (z2 && z3)
                {
                    SELF.SelectWorld(i1);
                }
            }

            protected override bool IsSelected(int i1)
            {
                return i1 == SELF.selectedWorld;
            }

            protected override int GetContentHeight()
            {
                return SELF.saveList.Count * 36;
            }

            protected override void DrawBackground()
            {
                SELF.DrawDefaultBackground();
            }

            protected override void DrawSlot(int i1, int i2, int i3, int i4, Tessellator tessellator5)
            {
                LevelSummary saveFormatComparator6 = SELF.saveList[i1];
                string string7 = saveFormatComparator6.GetDisplayName();
                if (string.IsNullOrEmpty(string7))
                {
                    string7 = SELF.field_22098_o + " " + (i1 + 1);
                }

                string string8 = saveFormatComparator6.GetFileName();
                //CBA TO DEAL WITH THIS JUST HAVE A RANDOM OBJECT TO STRING MOMENT
                string8 = string8 + " (" + TimeSpan.FromMilliseconds(saveFormatComparator6.GetLastPlayed()).ToString();
                long j9 = saveFormatComparator6.GetSizeOnDisk();
                string8 = string8 + ", " + j9 / 1024 * 100 / 1024 / 100F + " MB)";
                string string11 = "";
                if (saveFormatComparator6.Func_22161_d())
                {
                    string11 = SELF.field_22097_p + " " + string11;
                }

                SELF.DrawString(SELF.font, string7, i2 + 2, i3 + 1, 0xFFFFFF);
                SELF.DrawString(SELF.font, string8, i2 + 2, i3 + 12, 8421504);
                SELF.DrawString(SELF.font, string11, i2 + 2, i3 + 12 + 10, 8421504);
            }
        }

        public override void InitGui()
        {
            I18N stringTranslate1 = I18N.Instance;
            screenTitle = stringTranslate1.TranslateKey("selectWorld.title");
            field_22098_o = stringTranslate1.TranslateKey("selectWorld.world");
            field_22097_p = stringTranslate1.TranslateKey("selectWorld.conversion");
            LoadSaves();
            worldSlotContainer = new WorldSelectionList(this);
            worldSlotContainer.RegisterScrollButtons(buttons, 4, 5);
            InitButtons();
        }

        private void LoadSaves()
        {
            ILevelStorageSource iSaveFormat1 = mc.GetSaveLoader();
            saveList = (List<LevelSummary>)iSaveFormat1.GetLevelList();
            saveList.Sort();
            selectedWorld = -1;
        }

        protected virtual string GetSaveFileName(int i1)
        {
            return saveList[i1].GetFileName();
        }

        protected virtual string GetSaveName(int i1)
        {
            string string2 = saveList[i1].GetDisplayName();
            if (string.IsNullOrEmpty(string2))
            {
                I18N stringTranslate3 = I18N.Instance;
                string2 = stringTranslate3.TranslateKey("selectWorld.world") + " " + (i1 + 1);
            }

            return string2;
        }

        public virtual void InitButtons()
        {
            I18N stringTranslate1 = I18N.Instance;
            buttons.Add(buttonSelect = new Button(1, width / 2 - 154, height - 52, 150, 20, stringTranslate1.TranslateKey("selectWorld.select")));
            buttons.Add(buttonRename = new Button(6, width / 2 - 154, height - 28, 70, 20, stringTranslate1.TranslateKey("selectWorld.rename")));
            buttons.Add(buttonDelete = new Button(2, width / 2 - 74, height - 28, 70, 20, stringTranslate1.TranslateKey("selectWorld.delete")));
            buttons.Add(new Button(3, width / 2 + 4, height - 52, 150, 20, stringTranslate1.TranslateKey("selectWorld.create")));
            buttons.Add(new Button(0, width / 2 + 4, height - 28, 150, 20, stringTranslate1.TranslateKey("gui.cancel")));
            buttonSelect.enabled = false;
            buttonRename.enabled = false;
            buttonDelete.enabled = false;
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.enabled)
            {
                if (guiButton1.id == 2)
                {
                    string string2 = GetSaveName(selectedWorld);
                    if (string2 != null)
                    {
                        deleting = true;
                        I18N stringTranslate3 = I18N.Instance;
                        string string4 = stringTranslate3.TranslateKey("selectWorld.deleteQuestion");
                        string string5 = "'" + string2 + "' " + stringTranslate3.TranslateKey("selectWorld.deleteWarning");
                        string string6 = stringTranslate3.TranslateKey("selectWorld.deleteButton");
                        string string7 = stringTranslate3.TranslateKey("gui.cancel");
                        ConfirmScreen guiYesNo8 = new ConfirmScreen(this, string4, string5, string6, string7, selectedWorld);
                        mc.SetScreen(guiYesNo8);
                    }
                }
                else if (guiButton1.id == 1)
                {
                    SelectWorld(selectedWorld);
                }
                else if (guiButton1.id == 3)
                {
                    mc.SetScreen(new CreateWorldScreen(this));
                }
                else if (guiButton1.id == 6)
                {
                    mc.SetScreen(new RenameWorldScreen(this, GetSaveFileName(selectedWorld)));
                }
                else if (guiButton1.id == 0)
                {
                    mc.SetScreen(parentScreen);
                }
                else
                {
                    worldSlotContainer.ActionPerformed(guiButton1);
                }
            }
        }

        public virtual void SelectWorld(int i1)
        {
            mc.SetScreen(null);
            if (!selected)
            {
                selected = true;
                mc.gameMode = new SurvivalMode(mc);
                string string2 = GetSaveFileName(i1);
                if (string2 == null)
                {
                    string2 = "World" + i1;
                }

                mc.LoadLevel(string2, GetSaveName(i1), 0);
                mc.SetScreen(null);
            }
        }

        public override void DeleteWorld(bool z1, int i2)
        {
            if (deleting)
            {
                deleting = false;
                if (z1)
                {
                    ILevelStorageSource iSaveFormat3 = mc.GetSaveLoader();
                    iSaveFormat3.ClearAll();
                    iSaveFormat3.DeleteLevel(GetSaveFileName(i2));
                    LoadSaves();
                }

                mc.SetScreen(this);
            }
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            worldSlotContainer.DrawScreen(i1, i2, f3);
            DrawCenteredString(font, screenTitle, width / 2, 20, 0xFFFFFF);
            base.DrawScreen(i1, i2, f3);
        }
    }
}
