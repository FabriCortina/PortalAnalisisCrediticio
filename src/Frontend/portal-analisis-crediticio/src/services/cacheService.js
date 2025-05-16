const CACHE_PREFIX = 'pac_'
const DEFAULT_TTL = 5 * 60 * 1000 // 5 minutos

class CacheService {
  constructor() {
    this.cache = new Map()
  }

  // Generar una clave única para el caché
  generateKey(key) {
    return `${CACHE_PREFIX}${key}`
  }

  // Obtener un valor del caché
  get(key) {
    const cacheKey = this.generateKey(key)
    const cached = this.cache.get(cacheKey)
    
    if (!cached) return null
    
    // Verificar si el caché ha expirado
    if (Date.now() > cached.expiry) {
      this.cache.delete(cacheKey)
      return null
    }
    
    return cached.value
  }

  // Guardar un valor en el caché
  set(key, value, ttl = DEFAULT_TTL) {
    const cacheKey = this.generateKey(key)
    this.cache.set(cacheKey, {
      value,
      expiry: Date.now() + ttl
    })
  }

  // Eliminar un valor del caché
  delete(key) {
    const cacheKey = this.generateKey(key)
    this.cache.delete(cacheKey)
  }

  // Limpiar todo el caché
  clear() {
    this.cache.clear()
  }

  // Obtener un valor del caché o ejecutar una función
  async getOrSet(key, fn, ttl = DEFAULT_TTL) {
    const cached = this.get(key)
    if (cached !== null) {
      return cached
    }

    const value = await fn()
    this.set(key, value, ttl)
    return value
  }
}

export const cacheService = new CacheService() 