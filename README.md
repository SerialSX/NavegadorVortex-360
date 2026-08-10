# NavegadorVortex360

Protótipo funcional de navegador panorâmico interativo em 360°, desenvolvido em Unity para o desafio técnico do processo seletivo de estágio em Jogos do Laboratório Vortex (UNIFOR). Replica o funcionamento do Google Street View, permitindo navegação entre 10 imagens panorâmicas reais de um trecho da Av. Domingos Olímpio, Fortaleza-CE.

**Candidato:** João Arthur de Abreu Souza
**Engine:** Unity 6.5 (6000.5.7f1), template Universal 3D (URP)

---

## Requisitos implementados

- [x] Unity como engine (recomendado 6.1 LTS; usada versão 6.5, aceitável conforme edital)
- [x] Projeto estruturado em cenas diferentes (Menu e Navegador)
- [x] Navegação entre imagens via teclado (W/S, setas ↑/↓)
- [x] Navegação entre imagens via clique do mouse (botões Avançar/Voltar)
- [x] 10 imagens panorâmicas mapeadas e navegáveis (Av. Domingos Olímpio)
- [x] Build funcional para WebGL, testado em navegador real

**Bônus implementados:**
- [x] Feedback visual e sonoro nas ações (fade entre panoramas + som de transição)
- [x] Estilização da página/UI (identidade visual, tipografia e cores consistentes)

---

## Como rodar

Existem três formas de acessar o projeto, em ordem de recomendação:

### 1. GitHub Pages (recomendado — sem senha, direto no navegador)

🔗 **https://serialsx.github.io/NavegadorVortex-360/**

Abre direto no navegador, sem download nem configuração. Clique em "Iniciar" e navegue pelos panoramas.

### 2. itch.io (canal alternativo — acesso restrito)

🔗 **[link do projeto no itch.io aqui]**

Página com acesso restrito. Use a senha abaixo quando solicitado:

```
Senha: vortex2026/projeto
```

### 3. Rodar localmente pelo Unity Editor

1. Abrir a pasta do projeto no Unity Hub (versão 6.5 ou compatível)
2. Abrir a cena `Menu` e dar Play no Editor

### 4. Rodar o build WebGL localmente (sem Unity instalado)

1. Baixar/clonar o repositório
2. Servir a pasta `docs/` com um servidor local — **não funciona com duplo clique no `index.html`**, pois navegadores bloqueiam WebGL de arquivo local por segurança:
   - VS Code: extensão Live Server, botão "Go Live" com a pasta `docs/` aberta
   - Ou terminal: `python -m http.server`, executado de dentro da pasta `docs/`
3. Acessar `http://localhost:[porta]` no navegador

**Controles:**
- `W` / `S` ou setas `↑` / `↓`: avançar / voltar entre panoramas
- Clique nos botões "Avançar" / "Voltar" na tela
- Tecla `Alt`: libera o cursor para clicar na UI (clicar fora da UI trava o cursor de volta ao modo de olhar ao redor)

---

## Arquitetura resumida

- **Cena Menu:** tela inicial, carrega a cena Navegador ao clicar em "Iniciar"
- **Cena Navegador:** contém toda a lógica de navegação
  - `NavigationManager.cs`: controla o índice do panorama atual, troca o `Skybox` da cena e dispara a transição (fade + som)
  - `CameraLook.cs`: controle de câmera em primeira pessoa (olhar ao redor) e gerenciamento do cursor
- **Sistema de navegação orientado a dados:** array `Material[] panoramas`, indexado por posição — adicionar uma imagem nova é adicionar um slot no array, sem alterar a lógica
- **Decisão de arquitetura central:** as imagens panorâmicas são aplicadas diretamente como `Skybox` da cena (shader `Skybox/Panoramic`), não em uma esfera 3D com mesh. O shader já foi feito para essa finalidade — isso eliminou a necessidade de qualquer geometria extra e simplificou o projeto.

---

## Diário de Bordo — Uso de Inteligência Artificial

> Nota de contexto: comecei este desafio com quase **zero experiência prévia em Unity e C#**. A ferramenta de IA foi usada como apoio para explicar sobre a engine, ajudar na criação de projetos para aprendizado e debugar problemas em tempo real — não para gerar uma solução pronta sem entendimento.

### Ferramenta utilizada

**Claude (Anthropic)**, em conversa contínua ao longo de todo o desenvolvimento — desde os primeiros passos em Unity até os ajustes finais de UI, o bônus de transição, e a configuração de hospedagem do build (GitHub Pages e itch.io).

### Como foi usado

O fluxo de trabalho seguia sempre o mesmo padrão: reproduzir o erro ou a dúvida → descrever o problema real (com print do Console/Editor quando havia erro) → entender a causa antes de aplicar qualquer correção → testar visualmente no Editor ou no navegador → só então considerar resolvido.

A IA foi usada para:
- Gerar os scripts C# principais (`CameraLook.cs`, `NavigationManager.cs`, `MenuManager.cs`)
- Diagnosticar erros de compilação e de build
- Explicar conceitos de Unity desconhecidos no início do desafio (Skybox, Anchor, Rect Transform, Input System)
- Sugerir soluções de UX (ex: o esquema da tecla Alt para liberar o cursor sem quebrar a navegação em primeira pessoa)
- Diagnosticar e resolver problemas de hospedagem do build WebGL (GitHub Pages, MIME type do WebAssembly, configuração do itch.io)

A IA **não** foi usada para:
- Decidir a localização real das imagens panorâmicas (escolha pessoal, por praticidade de deslocamento)
- Definir a composição artística do menu (paleta de cores, escolha da imagem de fundo)

### Prompts importantes (exemplos representativos)

- *"Como assim, tenho que conectar duas imagens?"* — dúvida sobre o conceito central do desafio, resolvida com uma analogia de livro-jogo ("escolha sua aventura")
- Envio direto de mensagens de erro do Console (ex: erros `CS0101`, `CS0111`, `CS0229`) pedindo a causa raiz antes da correção
- *"O que eu faço com esse botão?"* — dúvidas pontuais de UI que precisaram de explicação de conceito, não só de código

### Dificuldades reais enfrentadas

1. **Confusão entre arquivos `.cs`:** colei o código de um script dentro do arquivo de outro por engano, gerando 11 erros de compilação simultâneos. Aprendizado: cada script MonoBehaviour precisa estar em seu próprio arquivo, com o nome do arquivo igual ao nome da classe.
2. **Entendimento do conceito central do desafio:** não ficou claro de imediato que "navegar entre imagens" significava simular deslocamento real (como o Street View), e não apenas trocar imagens soltas.
3. **Configuração de Git com Unity:** avisos de LF/CRLF e arquivos de cache sendo indexados por engano por `.gitignore` mal posicionado (incluindo uma correção necessária: regras genéricas como `Build/` no `.gitignore` acabavam ignorando também a pasta `docs/Build/`, usada de propósito para hospedar o build final).
4. **Erro de build WebGL não previsto:** compressão de arquivo (Gzip) quebrando o teste local com Live Server — resolvido isolando a causa por eliminação (`Compression Format = Disabled`).
5. **Hospedagem do build WebGL:** o GitHub Pages, por padrão, não serve arquivos `.wasm` com o cabeçalho HTTP correto, causando erro de carregamento mesmo com o build funcionando localmente — resolvido adicionando um arquivo `.nojekyll` na pasta de publicação. Também foi necessário contornar o limite de 25MB para upload de arquivos grandes pela interface web do GitHub, resolvido enviando o arquivo `.wasm` via linha de comando (`git push`), que não tem essa restrição.

### Como as respostas da IA foram validadas

Nenhuma sugestão de código ou configuração foi considerada "correta" apenas por parecer coerente — cada uma só foi aceita depois de testada visualmente no Unity Editor ou no navegador (build real). Quando um erro persistia, o processo era sempre voltar ao Console, capturar a mensagem exata, e reiniciar o ciclo de diagnóstico.

### Reflexão crítica

1. O que mudou do início pro fim?
   No momento que eu comecei a utilizar a Unity, eu tentei me familiarizar (Apesar de já ter utilizado o Godot Engine). Alguns conceitos foram fáceis de compreender, como "Anchor", a conexão entre imagens, mais por serem nomes e métodos parecidos com os que uso tanto no Godot, como até mesmo em alguns softwares de edição de vídeo. Mas tinha coisas que eu não fazia ideia, por falta de pratica e de nunca ter tentado usar a fundo a Unity. Entretanto, conforme o tempo foi passando, eu fui tirando algumas duvidas com a IA, fazendo alguns projetos pequenos pra treinar antes de eu fazer o protótipo verdadeiramente funcional para o Vortex-2026, aos poucos, as coisas foram ficando mais automáticas para mim, como erros pequenos que eu já corrigi-o sem nem perceber, como matérias colocados incorretamente em objetos não necessários, ou problemas com scripts que, muitas vezes, eram por causa de confusão na escrita (Nomes de variáveis ou virgulas má colocadas.).

2. Onde a IA acelerou vs. onde exigiu cuidado meu:
   Alguns scripts que fiz vieram com erros que não achava. Em momentos como esse eu enviava para a IA, perguntando se era algum problema de conflito ou de propriedades. Porém, algumas vezes ela mandava um script totalmente novo CHEIO de conflitos e variáveis ou scripts duplicados, sendo uma das tentativas um código completo com 11 erros de compilação. Eu voltei reescrevendo o código como estava antes chequei os erros dados, dei uma pesquisada e testei com outras formas mais simples, e uma delas funcionou.

3. Sugestão que precisou ser ajustada ou rejeitada:
   Tive varias ideias para serem implementadas neste projeto, como a capacidade de seguir para mais de uma direção ou até mesmo ir para uma destinada localização, porém com o tempo curto, a necessidade de um banco, e os requisitos que verifiquei no PDF proposto para o desafio, decidi deixar isso de lado e focar somente no necessário, ainda dando tempo de adicionar sons, e também um leve "Fade", nas transições entre imagens. Teve também ajustes contínuos para verificar localizações e posicionamento dos botões (O próprio fato de ter 3 formas de se movimentar, sendo "W" e "S", seta pra cima ou pra baixo, e os mesmos botões "Avançar" e "Voltar".)

4. Isso mudou minha visão sobre IA em projetos futuros?
   Sim e Não. Tive seis dias para fazer este projeto porque que só vim descobrir sobre ele um tempo depois de receber ele pelo Unifor Mobile dia 30 de julho. Acabei checando somente dia 2 de Agosto. Acredito sim que se tivesse mais tempo, eu teria feito bem mais coisa. Mas falando sobre o uso de IA, eu provavelmente utilizaria ela de forma mais "Técnica", para checagem de códigos, verificação de possíveis conflitos futuros, principalmente numa época onde a IA está constantemente crescendo, e a forma de utilizar ela DEVE ser explicada e categorizada como Assistente de Programação. Ela, apesar de capaz, não é melhor utilizada quando usada para gerar um código do zero, mas sim para otimizar o tempo para coisas pequenas no código ou, como disse anteriormente, para verificação geral no código ou tirar duvidas sobre algo muito especifico.

---

## Estrutura de causa e efeito (decisões técnicas)

| Problema encontrado | Decisão tomada | Por quê |
|---|---|---|
| Shader Skybox/Panoramic não funcionava em uma esfera 3D | Abandonar a esfera, usar o Skybox da cena diretamente | O shader é feito para isso — arquitetura mais simples, sem geometria |
| Erro de Input System (projeto novo usa API nova por padrão) | Mudar Active Input Handling para "Both" | Evita reescrever com a API nova; aviso de depreciação não afeta o build |
| Cursor travado impedia clicar em botões de UI | Tecla Alt libera o cursor; clique fora da UI trava de volta | Resolve o conflito entre "olhar tipo FPS" e "clicar em botão" |
| Clique no botão "Avançar" não registrava | Checar `IsPointerOverGameObject()` antes de travar o cursor | Separa as duas intenções de clique |
| Botões flutuavam para posições erradas em telas diferentes | Trocar Anchor do centro para Anchor Presets nos cantos reais | Ancora no canto de verdade, não simula com posição a partir do centro |
| Build comprimido (Gzip) quebrava ao testar localmente | Compression Format = Disabled | Necessário só para o teste local; não afeta o requisito em si |
| GitHub Pages não servia o arquivo `.wasm` corretamente | Adicionar arquivo `.nojekyll` na pasta de publicação | Desativa o processamento Jekyll, que interferia no cabeçalho HTTP do WebAssembly |
| Upload do `.wasm` (43MB) falhava pela interface web do GitHub (limite de 25MB) | Enviar via `git push` (linha de comando) em vez de upload manual | Git via terminal não tem o limite de 25MB da interface web |

---

## Mapa de conexões das imagens

Local: Av. Domingos Olímpio, Fortaleza — trajeto reto, em direção ao centro (a partir do cruzamento com R. Jaime Benevides). Sequência linear simples: `panorama_01` é o início, cada número seguinte é "um passo à frente" do anterior, até `panorama_10`.

---

## Créditos e ferramentas de terceiros

- Imagens panorâmicas obtidas via [Street View Download 360](https://svd360.istreetview.com/)
- Efeito sonoro de transição: Freesound.org
