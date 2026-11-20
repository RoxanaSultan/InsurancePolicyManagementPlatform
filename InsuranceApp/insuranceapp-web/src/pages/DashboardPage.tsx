// useEffect execută încărcarea clienților când se deschide pagina.
// useState păstrează valori care se pot schimba și actualizează interfața.
import { useEffect, useState } from 'react'

// useLocation citește informațiile trimise prin navigare.
// useNavigate schimbă pagina din cod.
import { useLocation, useNavigate } from 'react-router'

// getClients face cererea către API; Client descrie forma unui client.
import { getClients, type Client } from '../services/clientService'

// Aceasta este componenta afișată la ruta /dashboard.
export default function DashboardPage() {
  // Obținem funcția de navigare.
  const navigate = useNavigate()

  // Obținem informațiile rutei curente, inclusiv location.state.
  const location = useLocation()

  // Mesajul de succes este vizibil dacă pagina de creare a trimis
  // { clientCreated: true } prin navigate().
  const [showSuccess, setShowSuccess] = useState(
    location.state?.clientCreated === true,
  )

  // Lista începe goală și va fi completată cu răspunsul API-ului.
  const [clients, setClients] = useState<Client[]>([])

  // La început încărcarea este în curs.
  const [isLoading, setIsLoading] = useState(true)

  // Un șir gol înseamnă că nu avem nicio eroare de afișat.
  const [error, setError] = useState('')

  // Efectul pornește când componenta se afișează.
  useEffect(() => {
    // Definim o funcție async deoarece cererea către API durează.
    async function loadClients() {
      // Citim tokenul salvat la autentificare.
      const accessToken = sessionStorage.getItem('accessToken')

      // Fără token, utilizatorul trebuie să revină la login.
      if (!accessToken) {
        navigate('/login', { replace: true })
        return
      }

      // Încercăm să încărcăm clienții.
      try {
        // Așteptăm răspunsul API-ului.
        const data = await getClients()

        // Punem clienții primiți în state; React reafișează pagina.
        setClients(data)
      } catch (error) {
        // Dacă cererea eșuează, pregătim un mesaj pentru utilizator.
        setError(
          error instanceof Error
            ? error.message
            : 'A apărut o eroare neașteptată.',
        )
      } finally {
        // La final, indiferent de rezultat, încărcarea s-a terminat.
        setIsLoading(false)
      }
    }

    // Apelăm funcția; „void” indică faptul că nu folosim Promise-ul returnat.
    void loadClients()
  }, [navigate]) // Efectul depinde de funcția navigate.

  // Această funcție este apelată la apăsarea butonului „Deconectare”.
  function handleLogout() {
    // Eliminăm tokenul din sesiunea browserului.
    sessionStorage.removeItem('accessToken')

    // Mergem la login și înlocuim intrarea curentă din istoric.
    navigate('/login', { replace: true })
  }

  // return descrie interfața care se afișează.
  return (
    // Containerul general: bara laterală și conținutul principal.
    <div className="min-h-screen bg-slate-100 text-slate-800 lg:grid lg:grid-cols-[250px_1fr]">
      {/* Bara laterală. */}
      <aside className="flex min-h-full flex-col justify-between bg-slate-900 p-4 text-slate-200 lg:min-h-screen lg:p-5">
        <div>
          {/* Numele aplicației și simbolul „I”. */}
          <div className="flex items-center gap-3 px-3 py-2 text-xl font-extrabold text-white">
            <span className="grid size-8 place-items-center rounded-lg bg-blue-600 text-base">
              I
            </span>
            <span>InsuranceApp</span>
          </div>

          {/* Meniul lateral; aria-label îi dă un nume accesibil. */}
          <nav
            className="mt-8 grid gap-2"
            aria-label="Navigare principală"
          >
            {/* Elementul activ este Dashboard. */}
            <span className="rounded-lg bg-blue-800 px-3 py-3 font-bold text-white">
              ▦ Dashboard
            </span>

            {/* Următoarele trei elemente sunt momentan doar text. */}
            <span className="rounded-lg px-3 py-3 text-slate-400">
              ♙ Clienți
            </span>

            <span className="rounded-lg px-3 py-3 text-slate-400">
              ⌂ Clădiri
            </span>

            <span className="rounded-lg px-3 py-3 text-slate-400">
              ▤ Polițe
            </span>
          </nav>
        </div>

        {/* La clic, apelăm funcția de deconectare definită mai sus. */}
        <button
          className="mt-8 w-full rounded-lg border border-slate-700 px-3 py-2.5 font-semibold text-slate-200 transition hover:bg-slate-800"
          onClick={handleLogout}
        >
          Deconectare
        </button>
      </aside>

      {/* Conținutul principal al dashboardului. */}
      <main className="p-6 sm:p-8 lg:p-12">
        {/* „&&” afișează mesajul numai când showSuccess este true. */}
        {showSuccess && (
          // role="status" ajută tehnologiile de accesibilitate.
          <div
            role="status"
            className="mb-6 flex items-center justify-between gap-4 rounded-xl border border-green-200 bg-green-50 px-4 py-3 font-medium text-green-800"
          >
            <span>Clientul a fost adăugat cu succes.</span>

            {/* Butonul închide mesajul; type="button" nu trimite formulare. */}
            <button
              type="button"
              aria-label="Închide mesajul"
              onClick={() => {
                // Ascundem mesajul din interfață.
                setShowSuccess(false)

                // Ștergem și informația de succes din ruta curentă.
                navigate('/dashboard', { replace: true, state: null })
              }}
              className="rounded p-1 text-xl leading-none hover:bg-green-100 focus:outline-none focus:ring-2 focus:ring-green-600"
            >
              ×
            </button>
          </div>
        )}

        {/* Introducerea dashboardului. */}
        <header className="mb-9 flex items-start justify-between gap-6">
          <div>
            <p className="mb-2 text-xs font-extrabold tracking-[0.1em] text-blue-600">
              ZONA BROKER
            </p>

            <h1 className="text-3xl font-bold tracking-[-0.03em] text-slate-950 sm:text-4xl">
              Bun venit!
            </h1>

            <p className="mt-3 text-slate-500">
              Aici îți poți administra clienții și polițele de asigurare.
            </p>
          </div>

          {/* Avatarul „B” este deocamdată un element vizual. */}
          <div className="grid size-11 shrink-0 place-items-center rounded-full bg-blue-100 font-extrabold text-blue-700">
            B
          </div>
        </header>

        {/* Cele două carduri cu informații scurte. */}
        <section
          className="mb-7 grid gap-5 md:grid-cols-2"
          aria-label="Sumar"
        >
          {/* Primul card afișează numărul de clienți încărcați. */}
          <article className="grid gap-2 rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
            <span className="text-slate-500">Clienți înregistrați</span>

            <strong className="text-3xl font-bold text-slate-950">
              {clients.length}
            </strong>

            <span className="text-sm text-slate-500">
              în portofoliul tău
            </span>
          </article>

          {/* Al doilea card indică starea ultimei încărcări a listei. */}
          <article className="grid gap-2 rounded-2xl border border-slate-200 bg-white p-6 shadow-sm">
            <span className="text-slate-500">Conexiune API</span>

            {/* Alegem culoarea în funcție de existența unei erori. */}
            <strong
              className={
                error
                  ? 'text-3xl font-bold text-red-600'
                  : 'text-3xl font-bold text-green-700'
              }
            >
              {/* Alegem și textul în funcție de eroare. */}
              {error ? 'Eroare' : 'Activă'}
            </strong>

            <span className="text-sm text-slate-500">
              {/* În timpul cererii afișăm un text temporar. */}
              {isLoading ? 'Se verifică datele...' : 'date actualizate'}
            </span>
          </article>
        </section>

        {/* Secțiunea cu lista clienților. */}
        <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm">
          {/* Antetul secțiunii: titlu, explicație și buton. */}
          <div className="flex flex-col items-start justify-between gap-5 border-b border-slate-200 p-6 sm:flex-row sm:items-center">
            <div>
              <h2 className="text-xl font-bold text-slate-950">
                Clienți recenți
              </h2>

              <p className="mt-1.5 text-slate-500">
                Lista clienților asociați brokerului autentificat.
              </p>
            </div>

            {/* Butonul navighează către formularul de creare. */}
            <button
              type="button"
              onClick={() => navigate('/clients/new')}
              className="rounded-lg bg-blue-600 px-4 py-2 text-sm font-semibold text-white tranitions hover:bg-blue-700"
            >
              + Adaugă client
            </button>
          </div>

          {/* Mesaj afișat cât timp așteptăm API-ul. */}
          {isLoading && (
            <p className="p-6 text-slate-500">Se încarcă clienții...</p>
          )}

          {/* Mesaj afișat dacă încărcarea a eșuat. */}
          {error && (
            <p className="p-6 font-semibold text-red-700">{error}</p>
          )}

          {/* Mesaj afișat dacă încărcarea a reușit, dar lista e goală. */}
          {!isLoading && !error && clients.length === 0 && (
            <p className="p-6 text-slate-500">
              Nu există încă clienți înregistrați.
            </p>
          )}

          {/* Tabel afișat numai când există clienți și nu avem eroare. */}
          {!isLoading && !error && clients.length > 0 && (
            <div className="overflow-x-auto">
              <table className="w-full min-w-[620px] border-collapse text-left">
                {/* Numele coloanelor. */}
                <thead>
                  <tr className="border-b border-slate-200">
                    <th className="px-6 py-4 text-xs font-bold tracking-wide text-slate-500 uppercase">
                      Nume client
                    </th>
                    <th className="px-6 py-4 text-xs font-bold tracking-wide text-slate-500 uppercase">
                      Număr identificare
                    </th>
                    <th className="px-6 py-4 text-xs font-bold tracking-wide text-slate-500 uppercase">
                      Status
                    </th>
                  </tr>
                </thead>

                {/* Rândurile cu datele clienților. */}
                <tbody>
                  {/* map transformă fiecare client într-un rând de tabel. */}
                  {clients.map((client) => (
                    // key ajută React să identifice fiecare rând.
                    <tr
                      className="border-b border-slate-200 last:border-b-0"
                      key={client.id}
                    >
                      <td className="px-6 py-4 text-slate-700">
                        {client.name}
                      </td>

                      <td className="px-6 py-4 text-slate-700">
                        {client.identificationNumber}
                      </td>

                      {/* Acest status este momentan același pentru fiecare rând. */}
                      <td className="px-6 py-4">
                        <span className="rounded-full bg-green-100 px-2 py-1 text-xs font-bold text-green-700">
                          Înregistrat
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </section>
      </main>
    </div>
  )
}