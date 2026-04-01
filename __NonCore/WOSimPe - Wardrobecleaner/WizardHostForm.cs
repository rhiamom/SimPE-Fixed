using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using SimPe.Wizards;

namespace SimPe.Plugin
{
    public class WizardHostForm : Avalonia.Controls.Window
    {
        private Panel contentPanel;
        private Label lblMessage;
        private Button btnNext, btnBack, btnClose;
        private Stack<IWizardForm> history = new Stack<IWizardForm>();
        private IWizardForm currentStep;

        public WizardHostForm(IWizardForm firstStep)
        {
            BuildUI();
            ShowStep(firstStep);
        }

        void BuildUI()
        {
            Title = "Wardrobe Wrangler";
            Width = 600;
            Height = 480;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            lblMessage = new Label { Content = "", Margin = new Thickness(4, 4, 4, 0) };
            contentPanel = new Panel();

            btnBack  = new Button { Content = "< Back", Width = 80, Margin = new Thickness(2) };
            btnNext  = new Button { Content = "Next >", Width = 80, Margin = new Thickness(2) };
            btnClose = new Button { Content = "Close",  Width = 80, Margin = new Thickness(2) };

            btnClose.Click += (s, e) => Close();
            btnNext.Click  += (s, e) => GoNext();
            btnBack.Click  += (s, e) => GoBack();

            var buttonBar = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(4)
            };
            buttonBar.Children.Add(btnBack);
            buttonBar.Children.Add(btnNext);
            buttonBar.Children.Add(btnClose);

            var root = new DockPanel();
            DockPanel.SetDock(lblMessage, Dock.Top);
            DockPanel.SetDock(buttonBar, Dock.Bottom);
            root.Children.Add(lblMessage);
            root.Children.Add(buttonBar);
            root.Children.Add(contentPanel);

            Content = root;
        }

        void ShowStep(IWizardForm step)
        {
            var avPanel = step.WizardWindow;
            step.Init(OnStepChanged);
            currentStep = step;

            contentPanel.Children.Clear();
            _ = avPanel; // Avalonia panel embedding deferred — steps return null for now

            lblMessage.Content = step.WizardMessage;
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
            btnBack.IsEnabled = history.Count > 0;
            bool isFinal = currentStep.Next == null;
            btnNext.IsEnabled = currentStep.CanContinue && !isFinal;
            btnNext.Content = isFinal ? "Finish" : "Next >";
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
