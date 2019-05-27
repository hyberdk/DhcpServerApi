﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dhcp
{
    public class DhcpServerScopeReservationCollection : IEnumerable<DhcpServerScopeReservation>
    {
        public DhcpServer Server { get; }
        public DhcpServerScope Scope { get; }

        internal DhcpServerScopeReservationCollection(DhcpServerScope scope)
        {
            Server = scope.Server;
            Scope = scope;
        }

        /// <summary>
        /// Enumerates a list of reservations associated with the DHCP Scope
        /// </summary>
        public IEnumerator<DhcpServerScopeReservation> GetEnumerator()
            => DhcpServerScopeReservation.GetReservations(Scope).GetEnumerator();

        /// <summary>
        /// Enumerates a list of reservations associated with the DHCP Scope
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <summary>
        /// Creates a DHCP scope reservation from a client
        /// </summary>
        /// <param name="client">A DHCP client to convert to a reservation</param>
        /// <returns>The scope reservation</returns>
        public DhcpServerScopeReservation AddReservation(DhcpServerClient client)
        {
            if (!Scope.IpRange.Contains(client.IpAddress))
                throw new ArgumentOutOfRangeException(nameof(client), "The client address is not within the IP range of the scope");

            return DhcpServerScopeReservation.CreateReservation(Scope, client.IpAddress, client.HardwareAddress);
        }
        /// <summary>
        /// Creates a DHCP scope reservation
        /// </summary>
        /// <param name="address">IP Address to reserve</param>
        /// <param name="hardwareAddress">Hardware address (MAC address) of client associated with this reservation</param>
        /// <returns>The scope reservation</returns>
        public DhcpServerScopeReservation AddReservation(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress)
            => DhcpServerScopeReservation.CreateReservation(Scope, address, hardwareAddress);
        /// <summary>
        /// Creates a DHCP scope reservation
        /// </summary>
        /// <param name="address">IP Address to reserve</param>
        /// <param name="hardwareAddress">Hardware address (MAC address) of client associated with this reservation</param>
        /// <param name="allowedClientTypes">Protocols this reservation supports</param>
        /// <returns>The scope reservation</returns>
        public DhcpServerScopeReservation AddReservation(DhcpServerIpAddress address, DhcpServerHardwareAddress hardwareAddress, DhcpServerClientTypes allowedClientTypes)
            => DhcpServerScopeReservation.CreateReservation(Scope, address, hardwareAddress, allowedClientTypes);
        

        /// <summary>
        /// Deletes the specified scope
        /// </summary>
        /// <param name="scope">The scope to be deleted</param>
        /// <param name="retainClientDnsRecords">If true registered client DNS records are not removed. Useful in failover scenarios. Default = false</param>
        public void RemoveScope(DhcpServerScope scope, bool retainClientDnsRecords = false)
            => scope.Delete(retainClientDnsRecords);
    }
}
