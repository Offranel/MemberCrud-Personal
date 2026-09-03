using MemberCrud.Services;

namespace MemberCrud;

/// <summary>
/// Main application window for church administration.
///
/// Provides entry points to manage ministries and members. Event
/// handlers open the corresponding management forms.
/// </summary>
public partial class ChurchManagement : Form
{
    private readonly IMemberService _memberService = null!;

    /// <summary>
    /// Parameterless constructor required by the WinForms designer.
    /// Do not perform service-dependent wiring here.
    /// </summary>
    public ChurchManagement()
    {
            InitializeComponent();

            // Apply theme and header
            MemberCrud.UI.Theme.ApplyFormTheme(this);
            MemberCrud.UI.Theme.AddHeader(this, "Church Management");

            // Style navigation buttons
            MemberCrud.UI.Theme.StyleButton(MemberManagementBtn, MemberCrud.UI.ButtonStyleType.Primary);
            MemberCrud.UI.Theme.StyleButton(MinistryManagemetBtn, MemberCrud.UI.ButtonStyleType.Secondary);
    }

    /// <summary>
    /// Main constructor used at runtime to provide services.
    /// </summary>
    public ChurchManagement(IMemberService memberService) : this()
    {
        _memberService = memberService ?? throw new System.ArgumentNullException(nameof(memberService));
    }

    /// <summary>
    /// Opens the MinistryManagement form when the ministries button is clicked.
    /// </summary>
    /// <param name="sender">The control that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void MinistryManagemetBtn_Click(object sender, EventArgs e)
    {
        // Instantiate and display the MinistryManagement window.
        MinistryManagement ministryManagement = new MinistryManagement();
        ministryManagement.Show();
    }

    /// <summary>
    /// Opens the MemberManagement form when the members button is clicked.
    /// </summary>
    /// <param name="sender">The control that raised the event.</param>
    /// <param name="e">Event arguments.</param>
    private void MemberManagementBtn_Click(object sender, EventArgs e)
    {
        // Instantiate and display the MemberManagement window, forwarding the shared service.
        MemberManagement memberManagement = new MemberManagement(_memberService);
        memberManagement.Show();
    }
}
