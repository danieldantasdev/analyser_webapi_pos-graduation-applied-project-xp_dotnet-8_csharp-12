
# ProjetoAplicado - Código e Gráficos

## Como rodar a Web API (.NET 8)
1. Instale o .NET 8 SDK.
2. No terminal, vá até `src/WebApi` e execute:
   ```bash
   dotnet run
   ```
3. Abra o Swagger em: http://localhost:5000/swagger (ou porta exibida no console).

Endpoints:
- GET /api/static
- GET /api/dynamic
- GET /api/evolution
- GET /api/integration

## Como gerar as figuras (Python)
1. Garanta Python 3 com pandas, numpy, matplotlib instalados.
2. No terminal, vá até a pasta raiz do projeto e execute:
   ```bash
   python generate_charts.py
   ```
3. As imagens serão salvas em `figures/`:
   - fig2_cbo_vs_target.png
   - fig2_lcom_vs_target.png
   - fig2_cyclomatic_vs_target.png
   - fig2_inheritancedepth_vs_target.png
   - fig3_runtime_calls.png
   - fig4_hotspots_scatter.png
   - fig6_before_after.png
