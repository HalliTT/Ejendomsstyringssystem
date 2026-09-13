export interface Properties {
  id: string,
  name: string,
  address: string
}

export interface Property {
  id: string;
  name: string;
  address: string;
  city: string;
  country: string;
  description: string;
}

export interface Units {
  id: string;
  name: string;
  description: string;
}

// export interface Property {
//   id: string;
//   ownerId: string;
//   name: string;
//   address: string;
//   city: string;
//   country: string;
//   description: string;
//   isEnabled: boolean;
//   softDeletedAt: Date;
//   createdAt: Date;
//   updatedAt: Date;
// }