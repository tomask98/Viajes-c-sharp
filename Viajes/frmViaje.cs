using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//CURSO – LEGAJO – APELLIDO – NOMBRE

namespace TUP_PI_Recu_Viajes
{
    public partial class frmViaje : Form
    {
        AccesoDatos oBD;
        List<Viaje> LViaje;

        public frmViaje()
        {
            InitializeComponent();
            oBD = new AccesoDatos();
            LViaje = new List<Viaje>();
        }

        private void frmViaje_Load(object sender, EventArgs e)
        {
            cargarCombo();
            cargarLista();
            Habilitar(false);

        }

        private void Habilitar(bool v)
        {
            btnGrabar.Enabled = v;
            btnSalir.Enabled = !v;
            btnNuevo.Enabled = !v;
            txtCodigo.Enabled = v;
            txtDestino.Enabled = v;
            cboTransporte.Enabled = v;
            rbtInternacional.Enabled = v;
            rbtNacional.Enabled= v;
            dtpFecha.Enabled = v;
            lstViajes.Enabled = v;
        }

        private void cargarLista()
        {
            lstViajes.Items.Clear();
            LViaje.Clear();
            DataTable tabla = oBD.ConsultaBD("SELECT * FROM Viajes ");

            foreach (DataRow fila in tabla.Rows)
            {
                Viaje V = new Viaje();
                V.pCodigo = Convert.ToInt32(fila["codigo"]);
                V.pDestino = Convert.ToString(fila["destino"]);
                
                
                lstViajes.Items.Add(V);
                LViaje.Add(V);
            }

        }

        private void cargarCombo()
        {
            DataTable tabla = oBD.ConsultaBD("Select * from Transportes");

            cboTransporte.DataSource = tabla;
            cboTransporte.ValueMember = "idTransporte";
            cboTransporte.DisplayMember = "nombreTransporte";

            cboTransporte.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Habilitar(true);
            btnSalir.Enabled = true;
            Limpiar();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seguro que quiere salir", "Saliendo...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                Close();
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if(Validar())
            {
                Viaje V = new Viaje();
                V.pCodigo = Convert.ToInt32(txtCodigo);
                V.pDestino = txtCodigo.Text;
                V.pTransporte = Convert.ToInt32(cboTransporte.SelectedValue);
                if(rbtInternacional.Checked)
                {
                    V.pTipo = 2;
                }
                else
                {
                    V.pTipo = 1;
                }
                V.pFecha = dtpFecha.Value;
            if(!Existe(V))
             {
                string insertSQL = "INSERT INTO Viajes Values (@codigo,@destino,@transporte,@tipo,@fecha)";
                List<Parametro>lparametro = new List<Parametro>();
                lparametro.Add(new Parametro("@codigo", V.pCodigo));
                lparametro.Add(new Parametro("@destino", V.pDestino));
                lparametro.Add(new Parametro("@transporte", V.pTransporte));
                lparametro.Add(new Parametro("@tipo", V.pTipo));
                lparametro.Add(new Parametro("@fecha", V.pFecha));

                    if (oBD.ActualizarBD(insertSQL, lparametro) > 0)
                    {
                        MessageBox.Show("Viaje registrado exitosamente");
                        cargarLista();
                    }
             }
            else
                {
                    MessageBox.Show("El viaje ya existe");
                }
            }
        }

        private bool Existe(Viaje nuevo)
        {
            for (int i = 0; i < LViaje.Count; i++)
            {
                if(LViaje[i].pCodigo == nuevo.pCodigo)
                    return true;
            }
            return false;
            
        }

        private bool Validar()
        {
            bool valido = false;
            if(txtCodigo.Text== "")
            {
                MessageBox.Show("Debe ingresar el codigo");
                valido = true;
                
            }
            else 
                if(txtDestino.Text== "")
            {
                MessageBox.Show("Debe ingresar el destino");
                valido = true;

            }
            else
                if(cboTransporte.SelectedIndex == -1)
            {
                MessageBox.Show("Debe ingresar el Metodo de Transporte");
                valido = true;

            }
            else 
                 if(!rbtInternacional.Checked && !rbtNacional.Checked)
            {
                MessageBox.Show("Debe seleccionar el tipo de viaje");
                valido = true;
            }
            else
                if(dtpFecha.Value < DateTime.Today)
            {
                MessageBox.Show("fecha no valida");
                valido = true;

            }
                return valido;
        }

        private void Limpiar()
        {
            txtCodigo.Text = "";
            txtDestino.Text = "";
            cboTransporte.SelectedIndex = -1;
            rbtInternacional.Checked=false;
            rbtNacional.Checked=false;
            dtpFecha.Value = DateTime.Now;


        }
    }
}
