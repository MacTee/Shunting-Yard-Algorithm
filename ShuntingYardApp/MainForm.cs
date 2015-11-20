
namespace ShuntingYardApp
{
    using System;
    using System.Windows.Forms;
    using ShuntingYard;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            tbSource.Text = "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
            ActiveControl = tbSource;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ShowTokens();

                InfixToPostfixConverter conv = new InfixToPostfixConverter(new Tokenizer());
                var postfix = conv.Convert(tbSource.Text);
                tbPostfix.Text = postfix;
                
                PostfixEvaluator eva = new PostfixEvaluator();
                var result = eva.Evaluate(postfix);
                tbResult.Text = result.ToString();
            }
            catch (Exception ex)
            {
                tbResult.Text = string.Format("Error: {0}", ex.Message);
                tbTokenOutput.AppendText(tbResult.Text);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                this.button1_Click(tbSource, null);
        }

        private void ShowTokens()
        {
            Tokenizer tokenizer = new Tokenizer(tbSource.Text, new[] {'+', '-', '*', '/', '^', '(', ')'});
            Token currentToken = tokenizer.ReadNextToken();
            tbTokenOutput.Text = string.Empty;
            tbPostfix.Text = string.Empty;
            tbResult.Text = string.Empty;
            while (currentToken.Type != TokenType.None)
            {
                tbTokenOutput.AppendText(string.Format("Type: {0} - Value: '{1}'{2}", currentToken.GetTypeAsString(),
                    currentToken.GetValueAsString(), Environment.NewLine));
                currentToken = tokenizer.ReadNextToken();
            }
        }
    }
}
