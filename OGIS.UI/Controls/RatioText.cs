using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Richway.Ocean.Common.UserControls
{
    public partial class RatioText : UserControl
    {
        double _numNumerator=1;
        double _numDenominator=1;
        double _ratio = 1;
        bool _NumeratorVaild;
        bool _DenominatorVaild;
        Regex _reg = new Regex(@"^\+?[1-9][0-9]*$");

        public event EventHandler RatioChanged;

        public RatioText()
        {
            InitializeComponent();
            txtNumerator.Text = "1";
            txtDenominator.Text = "1";
            _numNumerator=1;
            _numDenominator=1;
            _NumeratorVaild = true;
            _DenominatorVaild = true;
            _ratio = 1;
        }

        public double Ratio { get { return _numDenominator==0?0:_numNumerator / _numDenominator; } }
        public double RatioNumerator { get { return _numNumerator; } }
        public double RatioDenominator { get { return _numDenominator; } }

        public bool Validable { get {return _NumeratorVaild&&_DenominatorVaild;}  }

        private bool Valid()
        {
            if (!_reg.IsMatch(txtNumerator.Text))
            {
                return false;
            }
            _numNumerator = double.Parse(txtNumerator.Text);
            if (!_reg.IsMatch(txtDenominator.Text))
            {
                return false;
            }
            _numDenominator = double.Parse(txtDenominator.Text);
            return true;
        }

        private void txtNumerator_TextChanged(object sender, EventArgs e)
        {
            _NumeratorVaild=_reg.IsMatch(txtNumerator.Text);
            if (!_NumeratorVaild)
            {
                MessageBox.Show("分子必须为非零正整数！");
                txtNumerator.Text = "1";
                _NumeratorVaild = true;
                return;
            }
            if (!double.TryParse(txtNumerator.Text,out _numNumerator))
            {
                MessageBox.Show("分子值无效");
                return;
            }
            RatioChanged.Invoke(this,null);
        }

        private void txtDenominator_TextChanged(object sender, EventArgs e)
        {
            _DenominatorVaild = _reg.IsMatch(txtDenominator.Text);
            if (!_DenominatorVaild)
            {
                MessageBox.Show("分母必须为非零正整数！");
                txtDenominator.Text = "1";
                _NumeratorVaild = true;
                return ;
            }
            if (!double.TryParse(txtDenominator.Text, out _numDenominator))
            {
                MessageBox.Show("分母值无效");
                return;
            }
            RatioChanged.Invoke(this, null);
        }
    }
}
