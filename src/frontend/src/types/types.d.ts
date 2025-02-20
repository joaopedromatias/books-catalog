type Filter = {
  authorName: ?string
  publishYear: ?number
  subjects: string[]
  title: ?string
  page: number
  pageSize: number
}

type Book = {
  id: number
  authorName: string
  publishYear: number
  subject: string
  title: string
  coverUri: string
}
