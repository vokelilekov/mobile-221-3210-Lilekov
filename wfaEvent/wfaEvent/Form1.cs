namespace wfaEvent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // (2)
            button2.Click += button2_Click;
            // (3)  анонимный метод или делегат
            button3.Click += delegate
            {
                MessageBox.Show("Способ 3");
            };
            // (4)
            button4.Click += (s, e) => MessageBox.Show("Способ 4");
        }

        private void button2_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Способ 2");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Способ 1");
        }
    }
}
