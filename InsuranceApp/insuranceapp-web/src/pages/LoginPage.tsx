import { type FormEvent, useState } from 'react'
import { useNavigate } from 'react-router'

type LoginResponse = {
  accessToken: string
}

export default function LoginPage() {
  const navigate = useNavigate()

  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [error, setError] = useState('')

  async function handleLogin(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()

    try {
      setIsSubmitting(true)
      setError('')

      const response = await fetch('https://localhost:7267/api/Auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
      })

      if (!response.ok) {
        throw new Error(
          response.status === 401
            ? 'Emailul sau parola sunt incorecte.'
            : 'Autentificarea nu poate fi efectuată acum.',
        )
      }

      const data: LoginResponse = await response.json()

      sessionStorage.setItem('accessToken', data.accessToken)
      setPassword('')
      navigate('/dashboard')
    } catch (error) {
      setError(
        error instanceof Error
          ? error.message
          : 'A apărut o eroare neașteptată.',
      )
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <main className="grid min-h-screen bg-slate-50 lg:grid-cols-[minmax(0,1.1fr)_minmax(420px,0.9fr)]">
      <section className="relative flex min-h-[280px] flex-col justify-between overflow-hidden bg-gradient-to-br from-slate-900 via-blue-950 to-blue-700 p-7 text-white sm:p-10 lg:min-h-screen lg:p-12">
        <div className="relative z-10 flex items-center gap-3 text-xl font-extrabold">
          <span className="grid size-9 place-items-center rounded-lg bg-white text-blue-600">
            I
          </span>
          <span>InsuranceApp</span>
        </div>

        <div className="relative z-10 mt-12 max-w-xl lg:mb-[15vh]">
          <p className="mb-4 text-xs font-extrabold tracking-[0.1em] text-blue-300">
            PLATFORMĂ PENTRU BROKERI
          </p>

          <h1 className="text-4xl leading-none font-bold tracking-[-0.04em] text-white sm:text-5xl lg:text-6xl">
            Asigurări gestionate simplu.
          </h1>

          <p className="mt-6 max-w-md text-base leading-relaxed text-slate-300 sm:text-lg">
            Administrează clienți, clădiri și polițe dintr-un singur loc.
          </p>
        </div>

        <div className="absolute -right-44 -bottom-40 size-[430px] rounded-full bg-white/10" />
        <div className="absolute top-[16%] right-[8%] size-64 rounded-full border border-white/15" />
      </section>

      <section className="grid place-items-center p-6 sm:p-12">
        <form className="w-full max-w-[420px]" onSubmit={handleLogin}>
          <div className="mb-8">
            <p className="mb-3 text-xs font-extrabold tracking-[0.1em] text-blue-600">
              AUTENTIFICARE
            </p>

            <h2 className="text-3xl font-bold tracking-[-0.03em] text-slate-900">
              Bine ai revenit
            </h2>

            <p className="mt-2 text-slate-500">
              Introdu datele contului tău de broker.
            </p>
          </div>

          <div className="grid gap-2">
            <label
              className="mt-2 text-sm font-bold text-slate-700"
              htmlFor="email"
            >
              Adresa de email
            </label>

            <input
              className="w-full rounded-lg border border-slate-300 bg-white px-3.5 py-3 text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-blue-600 focus:ring-4 focus:ring-blue-600/10"
              id="email"
              name="email"
              type="email"
              autoComplete="email"
              placeholder="nume@companie.ro"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
              required
            />

            <label
              className="mt-3 text-sm font-bold text-slate-700"
              htmlFor="password"
            >
              Parola
            </label>

            <input
              className="w-full rounded-lg border border-slate-300 bg-white px-3.5 py-3 text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-blue-600 focus:ring-4 focus:ring-blue-600/10"
              id="password"
              name="password"
              type="password"
              autoComplete="current-password"
              placeholder="Introdu parola"
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              required
            />
          </div>

          {error && (
            <p
              className="mt-5 rounded-lg bg-red-50 px-3 py-3 text-sm font-semibold text-red-700"
              role="alert"
            >
              {error}
            </p>
          )}

          <button
            className="mt-7 w-full rounded-lg bg-blue-600 px-4 py-3.5 font-bold text-white transition hover:-translate-y-0.5 hover:bg-blue-700 disabled:cursor-wait disabled:opacity-70"
            type="submit"
            disabled={isSubmitting}
          >
            {isSubmitting ? 'Se conectează...' : 'Conectare'}
          </button>

          <p className="mt-5 text-center text-sm leading-relaxed text-slate-500">
            Ai nevoie de acces? Contactează administratorul platformei.
          </p>
        </form>
      </section>
    </main>
  )
}