namespace wfaEvent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // (2)
            button2.Click += button2_Click;
            // (3)  ��������� ����� ��� �������
            button3.Click += delegate
            {
                MessageBox.Show("������ 3");
            };
            // (4)
            button4.Click += (s, e) => MessageBox.Show("������ 4");
        }

        private void button2_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("������ 2");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("������ 1");
        }
    }
}
