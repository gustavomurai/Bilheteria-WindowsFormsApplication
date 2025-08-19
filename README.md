# Bilheteria - Teatro/Cinema (Windows Forms)

## Autor

**Gustavo Murai** 
**Igor Murai** 

## Descrição do Projeto
Projeto desenvolvido em **C# usando Windows Forms Application** que simula a bilheteria de um teatro ou cinema com **600 poltronas**, distribuídas em 15 fileiras com 40 poltronas cada.  

O sistema permite controlar a **ocupação das poltronas**, consultar o **mapa de ocupação** e calcular o **faturamento atual** da bilheteria.

---

## Funcionalidades

O sistema possui um **seletor de opções** com as seguintes funcionalidades:

1. **Finalizar**  
   Encerra o programa.

2. **Reservar poltrona**  
   Permite reservar uma poltrona específica, informando **fileira** e **número da poltrona**.  
   - Se a poltrona estiver vaga: reserva com sucesso.  
   - Se a poltrona estiver ocupada: alerta o usuário.

3. **Mapa de ocupação**  
   Exibe um mapa visual das 600 poltronas:  
   - `.` = poltrona vaga  
   - `#` = poltrona ocupada  
   - Cores: verde = vaga, vermelho = ocupada  
   - Permite reservar ou liberar uma poltrona clicando diretamente no botão correspondente.

4. **Faturamento**  
   Exibe a quantidade de poltronas ocupadas e o valor total da bilheteria:  
   - Fileiras 1 a 5: R$ 50,00  
   - Fileiras 6 a 10: R$ 30,00  
   - Fileiras 11 a 15: R$ 15,00  

---

## Tela Inicial

Ao abrir o programa, você encontrará:  

- **ComboBox** com as opções 0 a 3.  
- **Botão Executar opção** para confirmar a escolha.  
- **Botão Faturamento** criado dinamicamente.  
- **Mini formulário** para reservar poltrona por número (fileira/poltrona).  
- **Painel do mapa** que será preenchido quando você clicar na opção "Mapa de ocupação".

---

## Como Executar

1. Abra o arquivo `Bilheteria.sln` no **Visual Studio**.  
2. Compile o projeto (`Build → Build Solution` ou **Ctrl+Shift+B**).  
3. Execute o programa (`Debug → Start Debugging` ou **F5**).  
4. Use o **seletor de opções** e os **botões** para interagir com o sistema.  

---

## Estrutura do Projeto

- `Form1.cs` → Código principal com toda a lógica do sistema.  
- `Form1.Designer.cs` → Arquivo gerado pelo Windows Forms Designer.  
- `Form1.resx` → Arquivo de recursos do Windows Forms.  
- Pastas `bin` e `obj` → Arquivos gerados pela compilação (não é necessário versionar no Git).  

---

## Tecnologias Utilizadas

- C#  
- Windows Forms  
- .NET Framework  
- Visual Studio  

---

## Autor

**Gustavo Murai** 
**Igor Murai** 

Projeto desenvolvido como exercício de **ADS** para implementar criação dinâmica de componentes em Windows Forms.


