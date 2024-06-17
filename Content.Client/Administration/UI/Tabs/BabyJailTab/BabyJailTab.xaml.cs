﻿using Content.Shared.Administration.Events;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Console;

/*
 * TODO: Remove me once a more mature gateway process is established. This code is only being issued as a stopgap to help with potential tiding in the immediate future.
 */

namespace Content.Client.Administration.UI.Tabs.BabyJailTab;

[GenerateTypedNameReferences]
public sealed partial class BabyJailTab : Control
{
    [Dependency] private readonly IConsoleHost _console = default!;

    private string _maxAccountAge;
    private string _maxOverallMinutes;

    public BabyJailTab()
    {
        RobustXamlLoader.Load(this);
        IoCManager.InjectDependencies(this);

        MaxAccountAge.OnTextEntered += args => SendMaxAccountAge(args.Text);
        MaxAccountAge.OnFocusExit += args => SendMaxAccountAge(args.Text);
        _maxAccountAge = MaxAccountAge.Text;

        MaxOverallMinutes.OnTextEntered += args => SendMaxOverallMinutes(args.Text);
        MaxOverallMinutes.OnFocusExit += args => SendMaxOverallMinutes(args.Text);
        _maxOverallMinutes = MaxOverallMinutes.Text;
    }

    private void SendMaxAccountAge(string text)
    {
        if (string.IsNullOrWhiteSpace(text) ||
            text == _maxAccountAge ||
            !int.TryParse(text, out var minutes))
        {
            return;
        }

        _console.ExecuteCommand($"babyjail_max_account_age {minutes}");
    }

    private void SendMaxOverallMinutes(string text)
    {
        if (string.IsNullOrWhiteSpace(text) ||
            text == _maxOverallMinutes ||
            !int.TryParse(text, out var minutes))
        {
            return;
        }

        _console.ExecuteCommand($"babyjail_max_overall_minutes {minutes}");
    }

    public void UpdateStatus(BabyJailStatus status)
    {
        EnabledButton.Pressed = status.Enabled;
        EnabledButton.Text = Loc.GetString(status.Enabled
            ? "admin-ui-baby-jail-enabled"
            : "admin-ui-baby-jail-disabled"
        );
        EnabledButton.ModulateSelfOverride = status.Enabled ? Color.Red : null;
        ShowReasonButton.Pressed = status.ShowReason;

        MaxAccountAge.Text = status.MaxAccountAgeMinutes.ToString();
        _maxAccountAge = MaxAccountAge.Text;

        MaxOverallMinutes.Text = status.MaxOverallMinutes.ToString();
        _maxOverallMinutes = MaxOverallMinutes.Text;
    }
}
