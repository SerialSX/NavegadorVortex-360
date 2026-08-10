# **Documentação Técnica — NavegadorVortex360**

**Projeto:** Navegador panorâmico interativo em 360° **Desafio:** Processo seletivo de estágio em Jogos — Laboratório Vortex (UNIFOR) **Candidato:** João Arthur de Abreu Souza **Engine:** Unity 6.5 (6000.5.7f1) — Template Universal 3D (URP)

---

## **1\. O que é o projeto**

Ele é basicamente um Google Street View caseiro. Dá pra navegar por 10 imagens panorâmicas reais de um trecho da Av. Domingos Olímpio, aqui em Fortaleza, como se estivesse andando pela rua.

A navegação é numa direção só (não tem cruzamento nem escolha de caminho) — cada imagem tem uma "antes" e uma "depois" na sequência. O usuário pode:

* Olhar ao redor em 360° arrastando o mouse (igual jogo em primeira pessoa)  
* Avançar/voltar entre as imagens, pelo teclado ou clicando nos botões  
  ---

  ## **2\. Cenas**

Duas cenas, como pede o edital:

* **Menu** — tela inicial com o título e o botão "Iniciar", que carrega a cena Navegador  
* **Navegador** — onde toda a navegação acontece

Transição entre elas é o básico do Unity: SceneManager.LoadScene().

---

## **3\. Como as pastas estão organizadas**

* Assets/  
*   ├── Panorama/     → as 10 imagens originais  
*   ├── Materials/     → os 10 materiais Skybox (Mat\_Panorama\_01 a 10\)  
*   ├── Scripts/        → os 3 scripts C\#  
*   ├── Audio/           → som de transição  
*   ├── Scenes/          → Menu.unity e Navegador.unity  
*   └── Settings/         → configs de URP  
    
  ---

  ## **4\. Os scripts**

São 3 scripts, cada um cuidando de uma coisa só.

### **4.1. MenuManager.cs**

O mais simples dos três. Só tem um método, que carrega a cena Navegador quando clica no botão Iniciar:

* public void IniciarNavegador()  
* {  
*     SceneManager.LoadScene("Navegador");  
* }


  ### **4.2. CameraLook.cs**

Cuida da câmera (olhar em primeira pessoa) e do cursor do mouse.

O cursor começa travado no centro da tela (CursorLockMode.Locked), o que deixa capturar o movimento do mouse sem ele sair da tela — igual em qualquer jogo FPS. O movimento vira rotação da câmera, com o eixo vertical limitado entre \-90° e 90° para não deixar a câmera virar de cabeça pra baixo.

A parte que exigiu mais atenção foi o cursor: apertando Alt, ele destrava e dá pra clicar na UI. Clicando fora de qualquer botão, ele trava de novo. Sem isso, ou você consegue olhar ao redor, ou consegue clicar nos botões — nunca os dois ao mesmo tempo, porque o cursor travado bloqueia interação com UI.

### **4.3. NavigationManager.cs**

O script principal, onde fica a lógica de qual panorama tá sendo mostrado e a transição entre eles.

* public Material\[\] panoramas;  
* private int currentIndex \= 0;


Os 10 materiais ficam num array, e um índice indica qual tá ativo agora. Avançar soma 1 no índice, voltar subtrai 1, com limite pros dois lados pra não passar do primeiro/último panorama.

A vantagem de fazer assim: pra adicionar uma imagem nova, é só colocar mais um material no array pelo Inspector. Não precisa mexer em nada do código.

O fluxo de uma troca de imagem:

1. Usuário aperta W/S, seta, ou clica num botão → chama GoForward() ou GoBack()  
2. Índice atualiza  
3. Isso dispara a coroutine FadeAndSwitch()  
4. A coroutine toca o som, escurece a tela, troca o Skybox por trás, e clareia de novo  
* IEnumerator FadeAndSwitch()  
* {  
*     if (audioSource \!= null && transitionSound \!= null)  
*         audioSource.PlayOneShot(transitionSound);  
*     yield return StartCoroutine(Fade(1f));  
*     RenderSettings.skybox \= panoramas\[currentIndex\];  
*     yield return StartCoroutine(Fade(0f));  
* }


Usei Coroutine porque o fade precisa acontecer ao longo de um tempo (0.25s), não instantâneo — é a forma padrão do Unity de fazer algo "esperar" entre frames sem travar o resto do jogo rodando.

---

## **5\. Por que Skybox e não uma esfera 3D**

A maioria dos tutoriais de navegador 360° usa uma esfera com a câmera dentro, e a imagem panorâmica é aplicada como textura na esfera. Não fiz assim.

Em vez disso, cada imagem é aplicada direto como o **Skybox da cena**, usando o shader Skybox/Panoramic do próprio Unity — que já é feito exatamente para esse tipo de imagem (equirretangular).

Por quê: tentei a abordagem da esfera primeiro, e o shader simplesmente não funciona certo aplicado numa mesh — ele é pensado só pra ser usado como Skybox mesmo. Trocando pra essa abordagem, não precisei de nenhuma geometria 3D na cena. A câmera sozinha já enxerga o panorama inteiro. Isso simplificou bastante o projeto comparado ao que os tutoriais mostram.

---

## **6\. UI**

**Cena Menu:** Canvas com imagem de fundo (um dos panoramas, estático), título "Vortex \- 2026" e botão Iniciar, ambos em TextMeshPro.

**Cena Navegador:** um Canvas pros botões Avançar/Voltar, ancorados nos cantos reais da tela (não no centro com posição grande — isso desalinhava em resoluções diferentes). E um segundo Canvas, separado, só pro fade — uma imagem preta cobrindo a tela toda, com CanvasGroup, e com Interactable/Blocks Raycasts desligados pra não travar clique nos botões durante a transição.

---

## **7\. Build WebGL**

Duas configs que precisaram de ajuste manual:

* **Compression Format \= Disabled** — sem isso, o build quebra ao testar local com Live Server (erro de sintaxe no carregamento)  
* **Active Input Handling \= Both** — o projeto usa a API clássica de Input; Unity novo vem configurado pra API nova por padrão, e isso quebra a compilação se não mudar

O build está hospedado em dois lugares: GitHub Pages (aberto, sem senha) e itch.io (acesso restrito por senha, como canal reserva).

Pro GitHub Pages funcionar com o arquivo .wasm, precisei adicionar um .nojekyll vazio na pasta publicada — sem isso, o processamento padrão do GitHub (Jekyll) mexe no cabeçalho HTTP do arquivo e ele não carrega no navegador.

---

## **8\. Mapa das imagens**

Sequência reta pela Av. Domingos Olímpio, do cruzamento com a R. Jaime Benevides em direção ao centro:

* panorama\_01 → panorama\_02 → **...** → panorama\_10


Sem ramificação, cada imagem só se conecta com a anterior e a próxima.

---

## **9\. Se fosse continuar**

Coisas que dariam para adicionar depois, sem precisar reescrever a base:

* Navegação em mais de uma direção por ponto  
* Ir direto pra um local específico (tipo clicando num minimapa)  
* Mais pontos no trajeto — só adicionar no array, a lógica já suporta