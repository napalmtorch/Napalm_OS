using System;
using System.Collections.Generic;
using System.Text;
using NapalmOS.Core;
using NapalmOS.Graphics;
using NapalmOS.GUI;
using NapalmOS.Hardware;

namespace NapalmOS.Programs
{
    public class WinTaskMan : Window
    {
        Button BtnEnd;
        ListBox ListTasks;
        public int UpdateInterval = 3;

        int time, timeOld, tick;
        int oldCount;
        public static bool Changed = false;
        private int indexOld;

        // constructor
        public WinTaskMan() : base(32, 32, 160, 120, "TASK MANAGER", "taskman.app")
        {
            this.ClientBoundsVisible = false;
            this.UpdateClientBounds();

            // end task button
            BtnEnd = new Button(ClientBounds.Width - 52, ClientBounds.Height - 13, "END TASK");
            AddControl(BtnEnd);

            // task list
            ListTasks = new ListBox(4, 11);
            ListTasks.SetSize(ClientBounds.Width - 8, ClientBounds.Height - 25);
            AddControl(ListTasks);

            // populate list
            PopulateList();
        }

        // update
        public override void Update()
        {
            base.Update();

            // re-populate list ever interval
            time = Cosmos.HAL.RTC.Second;
            if (time != timeOld) { tick++; timeOld = time; }
            if (tick >= UpdateInterval)
            {
                for (int i = 0; i < MemoryManager.Blocks.Count; i++)
                { 
                    MemoryManager.Blocks[i].UpdateUsage(false);
                }

                string text = "";
                for (int i = 0; i < ListTasks.Items.Count; i++)
                {
                    for (int j = 0; j < TaskManager.List.Count; j++)
                    {
                        if (ListTasks.Items[i].Tag1 == TaskManager.List[j].UID)
                        {
                            text = TaskManager.List[i].UID.PadRight(20, ' ') + i.ToString().PadRight(4, ' ') + TaskManager.List[i].MemoryUsed.ToString().PadRight(5, ' ') + " BYTES";
                            ListTasks.Items[i].Text = text;
                            ListTasks.Items[i].Tag1 = TaskManager.List[j].UID;
                            ListTasks.Items[i].Tag2 = TaskManager.List[j].Name;
                            break;
                        }
                    }
                }

                PopulateList();
                tick = 0; 
            }

            // disable button if system task
            if (ListTasks.SelectedIndex >= 0 && ListTasks.SelectedIndex < ListTasks.Items.Count)
            { BtnEnd.Enabled = !ListTasks.Items[ListTasks.SelectedIndex].Tag1.ToUpper().EndsWith(".SYS"); }

            // end task button clicked
            if (BtnEnd.MouseFlags.Down && !BtnEnd.MouseFlags.Clicked)
            {
                if (ListTasks.SelectedIndex >= 0 && ListTasks.SelectedIndex < ListTasks.Items.Count)
                { WindowManager.Close(ListTasks.Items[ListTasks.SelectedIndex].Tag1); }

                PopulateList();
                BtnEnd.MouseFlags.Clicked = true;
            }
        }

        // draw
        public override void Draw()
        {
            // background optimization hack - client bounds visible should be off
            if (!Flags.Moving)
            {
                WindowRenderer.DrawFilledRect(0, 0, ClientBounds.Width, 7, Style.Colors[0], this);
                WindowRenderer.DrawRect(0, 7, ClientBounds.Width, ClientBounds.Height - 14, 4, Style.Colors[0], this);
                WindowRenderer.DrawFilledRect(0, ClientBounds.Height - 14, ClientBounds.Width, 14, Style.Colors[0], this);
            }

            // draw base
            base.Draw();

            if (!Flags.Moving) 
            {
                // draw category text header
                WindowRenderer.DrawString(6, 2, "TASK                ID  MEM USED", Style.Colors[1], Font.Font3x5, this);

                // draw total memory usage
                uint used = 0;
                for (int i = 0; i < TaskManager.List.Count; i++)
                {
                    used += (TaskManager.List[i].MemoryUsed / 1024);
                }
                WindowRenderer.DrawString(4, BtnEnd.Y + 2, "TOTAL RAM: " + used.ToString() + "/" + (MemoryManager.Size / 1024).ToString() + " KB", Style.Colors[1], Font.Font3x5, this);
            }
        }

        // populate list with tasks
        private void PopulateList()
        {
            if (ListTasks.Items.Count != oldCount || Changed)
            {
                ListTasks.Items.Clear();
                for (int i = 0; i < TaskManager.List.Count; i++)
                {
                    string text = TaskManager.List[i].UID.PadRight(20, ' ') + i.ToString().PadRight(4, ' ') + TaskManager.List[i].MemoryUsed.ToString().PadRight(5, ' ') + " BYTES";
                    ListItem item = new ListItem(text);
                    item.Tag1 = TaskManager.List[i].UID;
                    item.Tag2 = TaskManager.List[i].Name;
                    ListTasks.Items.Add(item);
                }
                Changed = false;
                oldCount = ListTasks.Items.Count;
            }
        }
    }
}
