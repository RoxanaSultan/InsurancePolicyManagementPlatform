import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router'
import {
  createClient,
  type ClientType,
} from '../services/clientService'

export default function CreateClientPage() {
  const navigate = useNavigate()

  const [name, setName] = useState('')
  const [clientType, setClientType] = useState<ClientType>('Individual')
  const [identificationNumber, setIdentificationNumber] = useState('')
  const [email, setEmail] = useState('')
  const [phoneNumber, setPhoneNumber] = useState('')
  const [street, setStreet] = useState('')
  const [streetNumber, setStreetNumber] = useState('')
  const [error, setError] = useState('')
  const [isSaving, setIsSaving] = useState(false)

  const identificationLabel =
    clientType === 'Individual' ? 'CNP' : 'CUI'

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setError('')

    const cleanedName = name.trim()
    const cleanedIdentificationNumber = identificationNumber.trim()
    const cleanedEmail = email.trim()
    const cleanedPhoneNumber = phoneNumber.trim()
    const cleanedStreet = street.trim()
    const cleanedStreetNumber = streetNumber.trim()

    if (!cleanedName) {
      setError('Numele este obligatoriu.')
      return
    }

    if (
      clientType === 'Individual' &&
      !/^\d{13}$/.test(cleanedIdentificationNumber)
    ) {
      setError('CNP-ul trebuie să conțină exact 13 cifre.')
      return
    }

    if (
      clientType === 'Company' &&
      !/^\d{2,20}$/.test(cleanedIdentificationNumber)
    ) {
      setError('CUI-ul trebuie să conțină între 2 și 20 de cifre.')
      return
    }

    if (!/^\S+@\S+\.\S+$/.test(cleanedEmail)) {
      setError('Introdu o adresă de email validă.')
      return
    }

    if (!/^0\d{9}$/.test(cleanedPhoneNumber)) {
      setError('Telefonul trebuie să aibă 10 cifre și să înceapă cu 0.')
      return
    }

    if (Boolean(cleanedStreet) !== Boolean(cleanedStreetNumber)) {
      setError('Completează atât strada, cât și numărul sau lasă-le pe amândouă goale.')
      return
    }

    if (cleanedStreet && (cleanedStreet.length < 2 || cleanedStreet.length > 200)) {
      setError('Strada trebuie să aibă între 2 și 200 de caractere.')
      return
    }

    if (cleanedStreetNumber && (cleanedStreetNumber.length < 1 || cleanedStreetNumber.length > 20)) {
      setError('Numărul adresei poate avea maximum 20 de caractere.')
      return
    }

    try {
      setIsSaving(true)

      await createClient({
        name: cleanedName,
        identificationNumber: cleanedIdentificationNumber,
        clientType,
        contactInfo: {
          email: cleanedEmail,
          phoneNumber: cleanedPhoneNumber,
          ...(cleanedStreet && cleanedStreetNumber
            ? {
              address: {
                street: cleanedStreet,
                number: cleanedStreetNumber,
              },
            }
          : {}),
        },
      })

      navigate('/dashboard', {
        state: { clientCreated: true },
      })
    } catch (error) {
      setError(
        error instanceof Error
          ? error.message
          : 'A apărut o eroare la salvarea clientului.',
      )
    } finally {
      setIsSaving(false)
    }
  }

  return (
    <main className="min-h-screen bg-slate-100 px-4 py-10">
      <section className="mx-auto max-w-2xl rounded-2xl bg-white p-8 shadow-sm">
        <p className="text-sm font-semibold uppercase tracking-wide text-blue-600">
          Clienți
        </p>

        <h1 className="mt-2 text-3xl font-bold text-slate-900">
          Adaugă client
        </h1>

        <p className="mt-2 text-slate-600">
          Completează datele de identificare și de contact.
        </p>

        <form onSubmit={handleSubmit} className="mt-8 space-y-5">
          <div>
            <label htmlFor="name" className="mb-2 block text-sm font-medium text-slate-700">
              Nume complet / Denumire firmă
            </label>

            <input
              id="name"
              type="text"
              value={name}
              onChange={(event) => setName(event.target.value)}
              className="w-full rounded-lg border border-slate-300 px-3 py-2.5 outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
              placeholder="Ex: Andrei Popescu"
            />
          </div>

          <div>
            <label htmlFor="clientType" className="mb-2 block text-sm font-medium text-slate-700">
              Tip client
            </label>

            <select
              id="clientType"
              value={clientType}
              onChange={(event) =>
                setClientType(event.target.value as ClientType)
              }
              className="w-full rounded-lg border border-slate-300 bg-white px-3 py-2.5 outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
            >
              <option value="Individual">Persoană fizică</option>
              <option value="Company">Companie</option>
            </select>
          </div>

          <div>
            <label htmlFor="identificationNumber" className="mb-2 block text-sm font-medium text-slate-700">
              {identificationLabel}
            </label>

            <input
              id="identificationNumber"
              type="text"
              value={identificationNumber}
              onChange={(event) =>
                setIdentificationNumber(event.target.value)
              }
              className="w-full rounded-lg border border-slate-300 px-3 py-2.5 outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
              placeholder={
                clientType === 'Individual'
                  ? 'Introduceți CNP-ul'
                  : 'Introduceți CUI-ul'
              }
            />
          </div>

          <div>
            <label htmlFor="email" className="mb-2 block text-sm font-medium text-slate-700">
              Email
            </label>

            <input
              id="email"
              type="email"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
              className="w-full rounded-lg border border-slate-300 px-3 py-2.5 outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
              placeholder="client@exemplu.ro"
            />
          </div>

          <div>
            <label htmlFor="phoneNumber" className="mb-2 block text-sm font-medium text-slate-700">
              Număr de telefon
            </label>

            <input
              id="phoneNumber"
              type="tel"
              value={phoneNumber}
              onChange={(event) => setPhoneNumber(event.target.value)}
              className="w-full rounded-lg border border-slate-300 px-3 py-2.5 outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
              placeholder="07xxxxxxxx"
            />
          </div>

          <div>
            <label
              htmlFor="street"
              className="mb-2 block text-sm font-medium text-slate-700"
            >
              Stradă (opțional)
            </label>
            <input
              id="street"
              type="text"
              value={street}
              onChange={(event) => setStreet(event.target.value)}
              className="w-full rounded-lg border border-slate-300 px-3 py-2.5 outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
              placeholder="Ex: Strada Florilor"
            />
          </div>

          <div>
            <label
              htmlFor="streetNumber"
              className="mb-2 block text-sm font-medium text-slate-700"
            >
              Număr (opțional)
            </label>
            <input
              id="streetNumber"
              type="text"
              value={streetNumber}
              onChange={(event) => setStreetNumber(event.target.value)}
              className="w-full rounded-lg border-slate-300  px-3 py-2.5 outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
              placeholder="Ex: 12A"
            />
          </div>

          {error && (
            <p className="rounded-lg bg-red-50 px-3 py-2 text-sm text-red-700">
              {error}
            </p>
          )}

          <div className="flex flex-col-reverse gap-3 sm:flex-row sm:justify-end">
            <button
              type="button"
              onClick={() => navigate('/dashboard')}
              disabled={isSaving}
              className="rounded-lg border border-slate-300 px-4 py-3 font-semibold text-slate-700 transition hover:bg-slate-50 disabled:cursor-not-allowed disabled:opacity-60"
            >
                Anulează
            </button>

            <button
              type="submit"
              disabled={isSaving}
              className="rounded-lg bg-blue-600 px-4 py-3 font-semibold text-white transition hover:bg-blue-700 disabled:cursor-not-allowed disabled:bg-blue-300"
            >
              {isSaving ? 'Se salvează...' : 'Salvează clientul'}
            </button>
          </div>
        </form>
      </section>
    </main>
  )
}