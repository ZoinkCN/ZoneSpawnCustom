import * as esbuild from 'esbuild'

let ctx = await esbuild.context({
  entryPoints: ['./UI_debug.jsx'],
  bundle: true,
  outfile: 'debug/dist/debug.js',
})

let { host, port } = await ctx.serve({
  servedir: 'debug',
})