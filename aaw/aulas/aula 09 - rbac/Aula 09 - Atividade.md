# HANDOUT — AULA 09

## Projete a Matriz de Acesso

*SisAcad — sistema acadêmico fictício — Arquitetura de Aplicações Web*

## 🎯 MISSÃO

Vocês são o time de segurança contratado para definir QUEM pode fazer O QUÊ no SisAcad, o novo sistema acadêmico da faculdade. Os papéis já existem: Aluno, Professor, Coordenador e Secretaria.

- Preencham a matriz abaixo com ✓ (pode) ou ✗ (não pode) para cada papel × ação
- Justifiquem por escrito os 3 casos que geraram mais discussão na dupla
- Respondam à pergunta-armadilha da Parte 03 — ela esconde o limite do modelo

*⏱️ Tempo: 25 minutos  |  👥 Formato: em duplas  |  Regra de ouro: na dúvida, NEGUE o acesso.*

> **Nomes:** ____________________   **Turma:** ____________________   **Data:** ___ / ___ / ______

### Respostas sugeridas

## PARTE 01 — A matriz papel × permissão

Marquem ✓ (pode) ou ✗ (não pode) em cada célula:

| **Ação** | **Aluno** | **Professor** | **Coordenador** | **Secretaria** |
| --- | --- | --- | --- | --- |
| 1. Ver as próprias notas | ✓ | ✗ | ✗ | ✗ |
| 2. Ver notas de toda a turma | ✗ | ✓ | ✓ | ✗ |
| 3. Lançar nota | ✗ | ✓ | ✗ | ✗ |
| 4. Alterar nota já lançada | ✗ | ✓ | ✓ | ✗ |
| 5. Matricular aluno em disciplina | ✗ | ✗ | ✓ | ✓ |
| 6. Trancar matrícula | ✓ | ✗ | ✓ | ✓ |
| 7. Criar nova disciplina | ✗ | ✗ | ✓ | ✗ |
| 8. Ver dados pessoais de aluno | ✗ | ✗ | ✓ | ✓ |
| 9. Emitir histórico escolar | ✓ | ✗ | ✗ | ✓ |
| 10. Aprovar colação de grau | ✗ | ✗ | ✓ | ✗ |

**Limites de aplicação:** cada ✓ permite apenas a operação necessária ao papel, não acesso irrestrito. O aluno consulta ou tranca somente a própria matrícula e recebe apenas o próprio histórico. Professor acessa notas e as lança/retifica somente nas turmas e disciplinas sob sua responsabilidade; toda alteração deve ser justificada e auditada. Coordenador atua em casos acadêmicos sob sua alçada, especialmente correções excepcionais. Secretaria acessa somente os dados necessários aos processos de matrícula e documentação, com registro de acesso e observância da LGPD.

*Dica: pensem em consequências — quem responde perante o MEC se uma nota for alterada indevidamente? E perante a LGPD se dados pessoais vazarem?*

## PARTE 02 — Os casos polêmicos

Quais 3 células geraram mais discussão na dupla? Anotem a célula (ação + papel) e a justificativa da decisão final:

1. Caso 1 — **Alterar nota já lançada × Coordenador (✓):** pode ser necessário corrigir um erro ou tratar um recurso, mas a permissão deve se restringir a casos excepcionais, exigir justificativa e manter trilha de auditoria. O professor responsável continua podendo retificar notas de suas próprias turmas conforme as regras acadêmicas.

2. Caso 2 — **Trancar matrícula × Aluno (✓):** o aluno pode solicitar ou efetivar o trancamento da própria matrícula, respeitando prazos e regras institucionais. Não pode alterar matrículas de outras pessoas.

3. Caso 3 — **Emitir histórico escolar × Aluno (✓):** o aluno pode obter o próprio histórico por autoatendimento, sem poder emitir documentos de terceiros. A Secretaria também pode emitir o documento no processo oficial, com conferência e registro.

## PARTE 03 — A pergunta-armadilha

Vocês marcaram que Professor pode “lançar nota”. Mas… de QUALQUER turma? O professor de Cálculo pode lançar nota na turma de Arquitetura de Aplicações Web?

1. **Não.** O papel “Professor” autoriza a atividade em geral, mas não prova que aquela pessoa leciona naquela disciplina ou turma. Autorizar apenas pelo papel permitiria alterar notas de alunos fora de sua responsabilidade.

2. O sistema precisa conhecer a atribuição do professor à disciplina e à turma, além do contexto do registro: disciplina, turma/período e aluno matriculado. Deve verificar que a nota pertence a uma turma ativa sob responsabilidade daquele professor e que a ação está dentro das regras acadêmicas.

3. Na **Aula 07, no problema de autorização por objeto (IDOR/BOLA)**: estar autenticado ou ter um papel válido não basta; é preciso verificar se o usuário tem autorização para acessar aquele recurso específico. Aqui, além do RBAC, aplica-se ABAC, combinando atributos do professor, da turma e da nota.

## DESAFIO

1. Daria ao Monitor permissões limitadas à disciplina em que foi designado e somente durante o período da designação: consultar materiais e atividades da turma, apoiar a organização e, se necessário, registrar presença ou acompanhar entregas. Não permitiria lançar nem alterar notas oficiais, aprovar colação ou consultar dados pessoais e notas além do estritamente necessário. O novo papel cria o risco de um aluno obter acesso indevido a dados de colegas ou influenciar registros acadêmicos; por isso, o acesso deve ter escopo por turma, prazo de expiração, privilégio mínimo e auditoria.
