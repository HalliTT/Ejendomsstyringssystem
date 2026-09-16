export interface Properties {
  id: string,
  name: string,
  address: string
  occupiedUnits: number,
  totalUnits: number
}

export interface Property {
  id: string;
  name: string;
  address: string;
  city: string;
  country: string;
  description: string;
  occupiedUnits: number;
  totalUnits: number;
  units: Unit[];
}

export interface PropertyInput {
  name: string;
  address: string;
  city: string;
  country: string;
  description: string;
}

export interface Unit {
  id: string;
  name: string;
  description: string;
  status: UnitStatus;
}

export enum UnitStatus {
  Available,
  Occupied,
  Maintenance
}