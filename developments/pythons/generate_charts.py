
# generate_charts.py
# Creates the figures referenced in the report (bars, scatter, improvement chart, runtime calls diagram).
# Output images will be saved under ./figures
import os
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt

os.makedirs('figures', exist_ok=True)

# ---- Figure 1: Static metrics vs targets (bar chart) ----
systems = ['SistemaA','SistemaB','SistemaC']
metrics = pd.DataFrame({
    'System': systems,
    'CBO': [12.3, 11.1, 9.4],
    'LCOM': [0.68, 0.72, 0.66],
    'Cyclomatic': [8.7, 7.9, 9.1],
    'InheritanceDepth': [3.2, 2.9, 3.8]
})
targets = {'CBO':10.0,'LCOM':0.70,'Cyclomatic':10.0,'InheritanceDepth':5.0}

# Plot for CBO vs Target
for metric, target in targets.items():
    plt.figure()
    x = np.arange(len(systems))
    width = 0.35
    plt.bar(x - width/2, metrics[metric], width, label=metric)
    plt.bar(x + width/2, [target]*len(systems), width, label='Target')
    plt.xticks(x, systems)
    plt.ylabel(metric)
    plt.title(f'{metric} vs Target')
    plt.legend()
    plt.tight_layout()
    plt.savefig(f'figures/fig2_{metric.lower()}_vs_target.png', dpi=180)

# ---- Figure 2: Scatter of hotspots (changes vs complexity) ----
git = pd.read_csv('data/git.csv')
agg = git.groupby('file').agg({'changes':'sum','complexity':'mean'}).reset_index()
plt.figure()
plt.scatter(agg['changes'], agg['complexity'], s=40)
for _, row in agg.iterrows():
    plt.annotate(row['file'].split('/')[-1], (row['changes'], row['complexity']), fontsize=8)
plt.xlabel('Total Changes (churn)')
plt.ylabel('Avg Complexity')
plt.title('Hotspots: Changes vs Complexity')
plt.tight_layout()
plt.savefig('figures/fig4_hotspots_scatter.png', dpi=180)

# ---- Figure 3: Improvement chart (before/after) ----
labels = ['Coupling (CBO)', 'Cohesion', 'Delivery Time']
before = [12.3, 0.53, 100]   # arbitrary baseline
after  = [8.7, 0.74, 76]     # improvements (-31% CBO, +28% cohesion, -24% time)
plt.figure()
x = np.arange(len(labels))
width = 0.35
plt.bar(x - width/2, before, width, label='Before')
plt.bar(x + width/2, after, width, label='After')
plt.xticks(x, labels, rotation=15, ha='right')
plt.title('Before vs After Refactoring')
plt.tight_layout()
plt.legend()
plt.savefig('figures/fig6_before_after.png', dpi=180)

# ---- Figure 4: Runtime calls diagram (simple radial layout) ----
# We'll simulate node positions and draw arrows proportional to calls
endpoints = ['/api/orders','/api/payments','/api/users','/api/reviews']
calls = np.array([2300000, 1100000, 650000, 420000])
theta = np.linspace(0, 2*np.pi, len(endpoints), endpoint=False)
radius = 1.0
xs, ys = radius*np.cos(theta), radius*np.sin(theta)

plt.figure()
plt.scatter(xs, ys)
for i, ep in enumerate(endpoints):
    plt.annotate(ep, (xs[i], ys[i]), xytext=(5,5), textcoords='offset points', fontsize=9)
    # draw arrow from "UI" center to endpoint with linewidth scaled
    lw = 1 + (calls[i]/calls.max())*4
    plt.arrow(0, 0, xs[i]*0.9, ys[i]*0.9, length_includes_head=True, head_width=0.05, linewidth=lw)

plt.scatter([0],[0])
plt.annotate('UI', (0,0), xytext=(5,5), textcoords='offset points')
plt.title('Runtime Calls (thicker arrow = more calls)')
plt.axis('equal'); plt.axis('off')
plt.tight_layout()
plt.savefig('figures/fig3_runtime_calls.png', dpi=180)

print("Figures created in ./figures:")
print(os.listdir('figures'))
