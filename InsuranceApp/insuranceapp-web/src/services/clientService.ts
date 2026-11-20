const API_BASE_URL = 'https://localhost:7267/api'

export type Client = {
  id: number
  name: string
  identificationNumber: string
}

export type ClientType = 'Individual' | 'Company'

export type CreateClientRequest = {
  name: string
  identificationNumber: string
  contactInfo: {
    email:string
    phoneNumber: string
    address?: {
      street: string
      number:string
    }
  }
  clientType: ClientType
}

export async function getClients(): Promise<Client[]> {
  const accessToken = sessionStorage.getItem('accessToken')

  if (!accessToken) {
    throw new Error('Sesiunea a expirat.')
  }

  const response = await fetch(
    `${API_BASE_URL}/brokers/Clients?page=1&pageSize=20`,
    {
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    },
  )

  if (!response.ok) {
    throw new Error('Nu am putut încărca lista de clienți.')
  }

  return response.json() as Promise<Client[]>
}

export async function createClient(
  client: CreateClientRequest,
): Promise<Client> {
  const accessToken = sessionStorage.getItem('accessToken')

  if (!accessToken) {
    throw new Error('Sesiunea a expirat. Autentifică-te din nou.')
  }

  const response = await fetch(`${API_BASE_URL}/brokers/Clients`, {
    method:'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${accessToken}`,
    },
    body: JSON.stringify(client),
  })

  if (response.status ===409) {
    throw new Error('Există deja un client cu acest CNP/CUI, email sau număr de telefon.')
  }

  if (!response.ok) {
    throw new Error('Clientul nu a putut fi adăugat. Verifică datele introduse și încearcă din nou.')
  }

  return response.json() as Promise<Client>
}