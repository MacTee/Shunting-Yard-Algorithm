
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

            tbSource.Text = "(1)-(-1)"; // "1+(1)+(1)+1"; //"3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3";
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
                tbResult.Text = string.Format("Error: {0}{1}", ex.Message, Environment.NewLine);
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

        private void button2_Click(object sender, EventArgs e)
        {
            tbTokenOutput.AppendText(string.Format("PostfixEvaluation:{0}", Environment.NewLine));

            try
            {
                PostfixEvaluator eva = new PostfixEvaluator();
                var result = eva.Evaluate(tbPostfix.Text);
                tbResult.Text = result.ToString();
            }
            catch (Exception ex)
            {
                tbResult.Text = string.Format("Error: {0}{1}", ex.Message, Environment.NewLine);
                tbTokenOutput.AppendText(tbResult.Text);
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                this.button2_Click(tbSource, null);
        }
    }
}
