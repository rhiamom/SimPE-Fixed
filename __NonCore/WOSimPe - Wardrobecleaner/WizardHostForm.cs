using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SimPe.Wizards;

namespace SimPe.Plugin
{
    public class WizardHostForm : Form
    {
        Panel contentPanel;
        Label lblMessage;
        Button btnNext, btnBack, btnClose;
        Stack<IWizardForm> history = new Stack<IWizardForm>();
        IWizardForm currentStep;

        public WizardHostForm(IWizardForm firstStep)
        {
            BuildUI();
            ShowStep(firstStep);
        }

        void BuildUI()
        {
            this.Text = "Wardrobe Wrangler";
            this.Size = new System.Drawing.Size(600, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimizeBox = false;

            lblMessage = new Label { Dock = DockStyle.Top, Height = 24, Padding = new Padding(4, 4, 4, 0) };

            contentPanel = new Panel { Dock = DockStyle.Fill };

            var buttonBar = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 36,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(4)
            };

            btnClose = new Button { Text = "Close", Width = 80 };
            btnNext  = new Button { Text = "Next >", Width = 80 };
            btnBack  = new Button { Text = "< Back", Width = 80 };

            btnClose.Click += (s, e) => Close();
            btnNext.Click  += (s, e) => GoNext();
            btnBack.Click  += (s, e) => GoBack();

            buttonBar.Controls.AddRange(new Control[] { btnClose, btnNext, btnBack });

            this.Controls.Add(contentPanel);
            this.Controls.Add(lblMessage);
            this.Controls.Add(buttonBar);
        }

        void ShowStep(IWizardForm step)
        {
            // Access WizardWindow first so the step can create its lazy UI controls,
            // then Init() so it can call UpdateList() etc. on the now-existing controls.
            var panel = step.WizardWindow;
            step.Init(OnStepChanged);
            currentStep = step;

            contentPanel.Controls.Clear();
            panel.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(panel);

            lblMessage.Text = step.WizardMessage;
            UpdateButtons();
        }

        void OnStepChanged(IWizardForm sender, bool autonext)
        {
            UpdateButtons();
            if (autonext && sender.CanContinue && sender.Next != null)
                GoNext();
        }

        void UpdateButtons()
        {
            btnBack.Enabled = history.Count > 0;
            bool isFinal = currentStep.Next == null;
            btnNext.Enabled = currentStep.CanContinue && !isFinal;
            btnNext.Text = isFinal ? "Finish" : "Next >";
        }

        void GoNext()
        {
            var next = currentStep.Next;
            if (next != null)
            {
                history.Push(currentStep);
                ShowStep(next);
            }
            else
            {
                Close();
            }
        }

        void GoBack()
        {
            if (history.Count > 0)
                ShowStep(history.Pop());
        }
    }
}
