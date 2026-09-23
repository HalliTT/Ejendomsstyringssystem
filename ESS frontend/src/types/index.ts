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

export interface RentalOptionSummary {
  id: string;
  name: string;
  monthlyRent: number;
  status: RentalOptionStatus;
  propertyId: string;
  propertyName: string;
}

export interface Tenant {
  id: string;
  name: string;
  email: string;
  phone: string;
}

export type BookingStatus = "Upcoming" | "Active" | "Completed";

export interface Booking {
  id: string;
  rentalOptionId: string;
  rentalOptionName: string;
  propertyId: string;
  propertyName: string;
  tenantId: string;
  tenantName: string;
  tenantEmail: string;
  tenantPhone: string;
  startDate: string;
  endDate: string;
  status: BookingStatus;
}

export interface BookingFormInput {
  rentalOptionId: string;
  tenantId: string;
  startDate: string;
  endDate: string;
}