import { defineConfig, presetUno } from 'unocss'
import transformerDirectives from '@unocss/transformer-directives'
import presetIcons from '@unocss/preset-icons'

export default defineConfig({
  presets: [presetUno(), presetIcons()],
  transformers: [transformerDirectives()]
})
