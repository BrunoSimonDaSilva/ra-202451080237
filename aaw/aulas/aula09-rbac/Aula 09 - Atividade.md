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

**Limites:** cada permissão vale somente para a operação e os dados necessários ao papel. O aluno acessa apenas as próprias notas, matrícula e histórico. O professor atua apenas nas turmas e disciplinas sob sua responsabilidade; alterações de notas exigem justificativa e auditoria. Coordenador e Secretaria acessam dados pessoais apenas para suas atribuições, com registro de acesso e observância à LGPD.

*Dica: pensem em consequências — quem responde perante o MEC se uma nota for alterada indevidamente? E perante a LGPD se dados pessoais vazarem?*

## PARTE 02 — Os casos polêmicos

Quais 3 células geraram mais discussão na dupla? Anotem a célula (ação + papel) e a justificativa da decisão final:

1. Caso 1 — **Alterar nota já lançada × Coordenador (✓):** a permissão atende correções excepcionais e recursos acadêmicos; exige justificativa e trilha de auditoria. O professor também pode corrigir notas de suas turmas.

2. Caso 2 — **Trancar matrícula × Aluno (✓):** o aluno pode trancar somente a própria matrícula, respeitando prazos e regras institucionais; não pode alterar matrículas de terceiros.

3. Caso 3 — **Emitir histórico escolar × Aluno (✓):** o aluno pode obter o próprio histórico por autoatendimento. A Secretaria também pode emiti-lo no processo oficial, com conferência e registro.

## PARTE 03 — A pergunta-armadilha

Vocês marcaram que Professor pode “lançar nota”. Mas… de QUALQUER turma? O professor de Cálculo pode lançar nota na turma de Arquitetura de Aplicações Web?

1. **Não.** O papel “Professor” não comprova que aquela pessoa leciona na turma ou disciplina específica; autorizar somente pelo papel permitiria alterar notas fora de sua responsabilidade.

2. O sistema precisa conhecer a atribuição do professor à disciplina e à turma, o período letivo e a matrícula do aluno. Deve verificar que o registro pertence a uma turma ativa sob responsabilidade daquele professor.

3. Na **Aula 07**, no problema de autorização por objeto (IDOR/BOLA): autenticação e papel não bastam; é preciso validar o acesso ao recurso específico. Aqui, RBAC é complementado por ABAC, usando atributos do professor, da turma e da nota.

## DESAFIO

1. Daria ao Monitor permissões limitadas à disciplina e ao período da designação: consultar materiais e atividades e apoiar a organização; se necessário, registrar presença ou acompanhar entregas. Não permitiria lançar/alterar notas oficiais, aprovar colação ou consultar dados pessoais e notas de colegas além do estritamente necessário. O risco é um aluno obter acesso indevido a dados ou influenciar registros acadêmicos; aplicar privilégio mínimo, escopo por turma, expiração e auditoria.
