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
  rentalOptions: RentalOption[];
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

export interface UnitInput {
  name: string,
  description: string,
  status: UnitStatus
}

export enum UnitStatus {
  Available = "Available",
  Occupied = "Occupied",
  Maintenance = "Maintenance"
}


export type RentalOptionStatus = "Available" | "Unavailable";
export interface RentalOption {
  id: string;
  propertyId: string;
  name: string;
  monthlyRent: number;
  status: RentalOptionStatus;
  unitIds: string[];
}
export interface RentalOptionFormInput {
  name: string;
  monthlyRent: number;
  status: RentalOptionStatus;
  unitIds: string[];
}