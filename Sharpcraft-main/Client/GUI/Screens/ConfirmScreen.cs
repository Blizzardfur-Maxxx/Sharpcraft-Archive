namespace SharpCraft.Client.GUI.Screens
{
    public class ConfirmScreen : Screen
    {
        private Screen parentScreen;
        private string message1;
        private string message2;
        private string field_22106_k;
        private string field_22105_l;
        private int worldNumber;
        public ConfirmScreen(Screen guiScreen1, string string2, string string3, string string4, string string5, int i6)
        {
            this.parentScreen = guiScreen1;
            this.message1 = string2;
            this.message2 = string3;
            this.field_22106_k = string4;
            this.field_22105_l = string5;
            this.worldNumber = i6;
        }

        public override void InitGui()
        {
            this.buttons.Add(new SmallButton(0, this.width / 2 - 155 + 0, this.height / 6 + 96, this.field_22106_k));
            this.buttons.Add(new SmallButton(1, this.width / 2 - 155 + 160, this.height / 6 + 96, this.field_22105_l));
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            this.parentScreen.DeleteWorld(guiButton1.id == 0, this.worldNumber);
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.DrawDefaultBackground();
            this.DrawCenteredString(this.font, this.message1, this.width / 2, 70, 0xFFFFFF);
            this.DrawCenteredString(this.font, this.message2, this.width / 2, 90, 0xFFFFFF);
            base.DrawScreen(i1, i2, f3);
        }
    }
}