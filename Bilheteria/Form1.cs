using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bilheteria
{
    public partial class Form1 : Form
    {
        // Constantes do teatro
        const int QtdFileiras = 15;
        const int QtdPoltronas = 40;

        // Estado das poltronas: false = vago, true = ocupado
        bool[,] poltronas = new bool[QtdFileiras, QtdPoltronas];

        // Componentes criados dinamicamente
        Panel panelTopo;
        Panel panelMapa;
        ComboBox cbOpcao;
        Button btnExecutar;
        Button btnFaturamento;   // pedido do enunciado (criado dinamicamente)
        Label lblLegenda;
        GroupBox grpReserva;     // mini formulário para reservar por número
        Label lblFaturamento;    // criado dinamicamente ao clicar faturamento

        public Form1()
        {
            InitializeComponent();

            // Configurações básicas da janela
            this.Text = "Bilheteria - Teatro/Cinema (15 fileiras x 40 poltronas = 600 lugares)";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = 1200;   // largura maior para caber mapa
            this.Height = 800;

            // Monta toda a interface via código (dinamicamente)
            MontarInterface();
        }

        /// <summary>
        /// Cria a área de topo (seletor, botões, campo de reserva) e a área do mapa (scroll).
        /// </summary>
        private void MontarInterface()
        {
            // Painel superior
            panelTopo = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                Padding = new Padding(10)
            };
            this.Controls.Add(panelTopo);

            // Rótulo do seletor
            var lblSelecao = new Label
            {
                Text = "Seletor de opções:",
                AutoSize = true,
                Location = new Point(10, 10)
            };
            panelTopo.Controls.Add(lblSelecao);

            // ComboBox com as opções 0 a 3
            cbOpcao = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(10, 35),
                Width = 200
            };
            cbOpcao.Items.Add("0 - Finalizar");
            cbOpcao.Items.Add("1 - Reservar poltrona");
            cbOpcao.Items.Add("2 - Mapa de ocupação");
            cbOpcao.Items.Add("3 - Faturamento");
            cbOpcao.SelectedIndex = 1; // começa no "Reservar poltrona"
            panelTopo.Controls.Add(cbOpcao);

            // Botão Executar opção
            btnExecutar = new Button
            {
                Text = "Executar opção",
                Location = new Point(220, 34),
                Width = 130
            };
            btnExecutar.Click += BtnExecutar_Click;
            panelTopo.Controls.Add(btnExecutar);

            // Botão Faturamento (criado dinamicamente conforme pedido)
            btnFaturamento = new Button
            {
                Text = "Faturamento",
                Location = new Point(360, 34),
                Width = 120
            };
            btnFaturamento.Click += BtnFaturamento_Click;
            panelTopo.Controls.Add(btnFaturamento);

            // Legenda
            lblLegenda = new Label
            {
                AutoSize = true,
                Text = "Legenda: '.' = vago    '#' = ocupado    (no mapa, clique na poltrona para reservar/liberar)",
                Location = new Point(10, 70)
            };
            panelTopo.Controls.Add(lblLegenda);

            // Mini formulário para reservar por número (fileira/poltrona)
            CriarPainelReserva();

            // Painel para o mapa de 600 lugares (scrollable)
            panelMapa = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.White
            };
            this.Controls.Add(panelMapa);
        }

        /// <summary>
        /// GroupBox com NumericUpDown para Fileira/Poltrona e um botão "Reservar".
        /// </summary>
        private void CriarPainelReserva()
        {
            grpReserva = new GroupBox
            {
                Text = "Reservar poltrona por número",
                Location = new Point(520, 10),
                Size = new Size(650, 95)
            };
            panelTopo.Controls.Add(grpReserva);

            var lblFileira = new Label
            {
                Text = "Fileira (1 a 15):",
                Location = new Point(15, 30),
                AutoSize = true
            };
            grpReserva.Controls.Add(lblFileira);

            var numFileira = new NumericUpDown
            {
                Name = "numFileira",
                Minimum = 1,
                Maximum = QtdFileiras,
                Value = 1,
                Location = new Point(110, 28),
                Width = 60
            };
            grpReserva.Controls.Add(numFileira);

            var lblPoltrona = new Label
            {
                Text = "Poltrona (1 a 40):",
                Location = new Point(190, 30),
                AutoSize = true
            };
            grpReserva.Controls.Add(lblPoltrona);

            var numPoltrona = new NumericUpDown
            {
                Name = "numPoltrona",
                Minimum = 1,
                Maximum = QtdPoltronas,
                Value = 1,
                Location = new Point(295, 28),
                Width = 60
            };
            grpReserva.Controls.Add(numPoltrona);

            var btnReservar = new Button
            {
                Text = "Reservar",
                Location = new Point(370, 26),
                Width = 90
            };
            btnReservar.Click += (s, e) =>
            {
                int f = (int)numFileira.Value;
                int p = (int)numPoltrona.Value;
                ReservarPoltrona(f - 1, p - 1);
            };
            grpReserva.Controls.Add(btnReservar);
        }

        /// <summary>
        /// Trata o clique no botão "Executar opção" (0 a 3).
        /// </summary>
        private void BtnExecutar_Click(object sender, EventArgs e)
        {
            switch (cbOpcao.SelectedIndex)
            {
                case 0: // 0 - Finalizar
                    this.Close();
                    break;

                case 1: // 1 - Reservar poltrona
                    MessageBox.Show("Use o quadro 'Reservar poltrona por número' para informar a Fileira e a Poltrona e clique em 'Reservar'.");
                    break;

                case 2: // 2 - Mapa de ocupação
                    DesenharMapa();
                    break;

                case 3: // 3 - Faturamento
                    BtnFaturamento_Click(sender, e);
                    break;
            }
        }

        /// <summary>
        /// Desenha/Redesenha os 600 botões de poltronas (15 x 40).
        /// </summary>
        private void DesenharMapa()
        {
            panelMapa.SuspendLayout();
            panelMapa.Controls.Clear();

            // Tamanho e espaçamento dos botões do mapa
            int tamanho = 24;   // largura/altura do botão
            int margem = 4;     // espaço entre botões
            int offsetLeft = 10;
            int offsetTop = 10;

            for (int i = 0; i < QtdFileiras; i++)
            {
                for (int j = 0; j < QtdPoltronas; j++)
                {
                    var btn = new Button
                    {
                        Width = tamanho,
                        Height = tamanho,
                        Left = j * (tamanho + margem) + offsetLeft,
                        Top = i * (tamanho + margem) + offsetTop,
                        Tag = new Point(i, j), // guardo a posição: (fileira, poltrona)
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font(FontFamily.GenericMonospace, 8, FontStyle.Bold)
                    };

                    AtualizarBotaoPoltrona(btn, poltronas[i, j]);

                    var tt = new ToolTip();
                    tt.SetToolTip(btn, $"Fileira {i + 1}, Poltrona {j + 1}");

                    btn.Click += AoClicarPoltrona;

                    panelMapa.Controls.Add(btn);
                }
            }

            panelMapa.ResumeLayout();
        }

        /// <summary>
        /// Atualiza visualmente um botão de poltrona com base no estado (vago/ocupado).
        /// </summary>
        private void AtualizarBotaoPoltrona(Button btn, bool ocupada)
        {
            // Atendendo à legenda do enunciado:
            // '.' = vago; '#' = ocupado
            btn.Text = ocupada ? "#" : ".";
            btn.BackColor = ocupada ? Color.LightCoral : Color.LightGreen;
        }

        /// <summary>
        /// Clique direto na poltrona do mapa: reservar se vago; se ocupado, perguntar se deseja liberar.
        /// </summary>
        private void AoClicarPoltrona(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var pos = (Point)btn.Tag;
            int i = pos.X; // fileira (0-based)
            int j = pos.Y; // poltrona (0-based)

            if (!poltronas[i, j])
            {
                poltronas[i, j] = true;
                AtualizarBotaoPoltrona(btn, true);
                MessageBox.Show($"Poltrona reservada com sucesso!\nFileira {i + 1}, Poltrona {j + 1}.");
            }
            else
            {
                var resp = MessageBox.Show(
                    $"Essa poltrona já está ocupada.\nDeseja liberar (cancelar a reserva)?",
                    "Lugar ocupado",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resp == DialogResult.Yes)
                {
                    poltronas[i, j] = false;
                    AtualizarBotaoPoltrona(btn, false);
                    MessageBox.Show($"Poltrona liberada.\nFileira {i + 1}, Poltrona {j + 1}.");
                }
            }
        }

        /// <summary>
        /// Reserva por número (usando os NumericUpDown do painel de reserva).
        /// Aplica as validações de intervalo e de ocupado/vago.
        /// </summary>
        private void ReservarPoltrona(int fileiraIndex, int poltronaIndex)
        {
            // Validação de intervalo
            if (fileiraIndex < 0 || fileiraIndex >= QtdFileiras ||
                poltronaIndex < 0 || poltronaIndex >= QtdPoltronas)
            {
                MessageBox.Show("Fileira ou Poltrona inválida! Valores fora do intervalo permitido.");
                return;
            }

            // Verifica se está vago
            if (!poltronas[fileiraIndex, poltronaIndex])
            {
                poltronas[fileiraIndex, poltronaIndex] = true;
                MessageBox.Show($"Reserva efetuada com sucesso!\nFileira {fileiraIndex + 1}, Poltrona {poltronaIndex + 1}.");
            }
            else
            {
                MessageBox.Show("Esse lugar já se encontra ocupado!");
            }

            // Se o mapa já estiver desenhado, atualiza o botão correspondente
            foreach (Control c in panelMapa.Controls)
            {
                if (c is Button b && b.Tag is Point pt &&
                    pt.X == fileiraIndex && pt.Y == poltronaIndex)
                {
                    AtualizarBotaoPoltrona(b, poltronas[fileiraIndex, poltronaIndex]);
                    break;
                }
            }
        }

        /// <summary>
        /// Calcula e exibe o faturamento atual (botão + label criados dinamicamente).
        /// </summary>
        private void BtnFaturamento_Click(object sender, EventArgs e)
        {
            int ocupados = 0;
            decimal valor = 0m;

            for (int i = 0; i < QtdFileiras; i++)
            {
                for (int j = 0; j < QtdPoltronas; j++)
                {
                    if (poltronas[i, j])
                    {
                        ocupados++;

                        // Preços por faixa de fileira (i é 0-based)
                        if (i < 5) valor += 50m; // Fileiras 1 a 5
                        else if (i < 10) valor += 30m; // Fileiras 6 a 10
                        else valor += 15m; // Fileiras 11 a 15
                    }
                }
            }

            // Se já houver um label de faturamento, remove para recriar (evita duplicar)
            if (lblFaturamento != null && !lblFaturamento.IsDisposed)
            {
                panelTopo.Controls.Remove(lblFaturamento);
                lblFaturamento.Dispose();
            }

            // Cria o Label dinamicamente conforme o enunciado
            lblFaturamento = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, 92),
                Text = $"Qtde de lugares ocupados: {ocupados}\nValor da bilheteria: R$ {valor:N2}"
            };
            panelTopo.Controls.Add(lblFaturamento);

            // Mensagem auxiliar (opcional)
            MessageBox.Show($"Ocupados: {ocupados}\nFaturamento: R$ {valor:N2}", "Faturamento atual");
        }
    }
}
