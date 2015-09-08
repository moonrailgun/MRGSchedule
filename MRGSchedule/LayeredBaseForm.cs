using LayeredSkin.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MRGSchedule
{
    public partial class LayeredBaseForm : LayeredForm
    {
        public LayeredBaseForm()
        {
            InitializeComponent();

            this.BackgroundRender = new ShadowBackgroundRender();
        }
    }
}
